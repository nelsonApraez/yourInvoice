///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.ExcelModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.DIAN.Function.Constant;
using yourInvoice.DIAN.Function.Model;
using System.Data;
using System.Text;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.DIAN.Function.Business
{
    public class IntegrationDianBusiness : IIntegrationDianBusiness
    {
        private readonly IStorage storage;
        private readonly IFileOperation file;
        private readonly IEventNotificationRepository eventNotificationRepository;
        private readonly IInvoiceEventRepository invoiceEventRepository;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IFtp ftp;
        private readonly IUnitOfWorkCommonEF unitOfWork;
        private readonly ILogger<ValidationInvoiceIntegrationDian> logger;
        private AsyncRetryPolicy<byte[]> policyRetryFtpGetFile;
        private readonly int countRetryCountFtp;
        private readonly int retrySecondExecution;
        private const string Subject = "Proceso ‎ de ‎ validación ‎ con ‎ la ‎ DIAN ‎ finalizado ‎ para‎‎ ‎ ‎oferta ";
        private const string messageNotFileFtp = "No se pudo obtener archivo [Exitoso, Rechazado, Fallido] en el tiempo configurado FTP";
        private const int countRetryReject = 2;
        private const int countRetryFalled = 1;

        public IntegrationDianBusiness(IStorage storage, IFileOperation file, IEventNotificationRepository eventNotificationRepository,
            IInvoiceEventRepository invoiceEventRepository, IInvoiceRepository invoiceRepository, IFtp ftp, IUnitOfWorkCommonEF unitOfWork,
            ILogger<ValidationInvoiceIntegrationDian> logger, ICatalogBusiness catalogBusiness)
        {
            this.storage = storage;
            this.file = file;
            this.eventNotificationRepository = eventNotificationRepository;
            this.invoiceEventRepository = invoiceEventRepository;
            this.invoiceRepository = invoiceRepository;
            this.ftp = ftp;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            countRetryCountFtp = Convert.ToInt16(Environment.GetEnvironmentVariable("CountRetryCountFtp") ?? "10");
            retrySecondExecution = Convert.ToInt16(Environment.GetEnvironmentVariable("RetrySecondExecution") ?? "6");
            countRetryCountFtp = countRetryCountFtp > 30 || countRetryCountFtp <= 0 ? 10 : countRetryCountFtp;
            retrySecondExecution = retrySecondExecution > 6 || retrySecondExecution <= 0 ? 6 : retrySecondExecution;
            _catalogBusiness = catalogBusiness;
        }

        public async Task<bool> ValidationProcessDIAN(ServiceBusReceivedMessage message, ServiceBusMessageActions messageAction)
        {
            var dataCufe = Encoding.UTF8.GetString(message.Body);
            var modelCufe = JsonConvert.DeserializeObject<EnvoiceCufe>(dataCufe);
            await messageAction.CompleteMessageAsync(message);
            if (modelCufe is null || modelCufe.Envoices is null || modelCufe.Envoices?.Count <= 0)
            {
                return false;
            }
            int totalCufe = modelCufe.Envoices?.Count ?? 0;
            var pathFile = $"storage/{modelCufe.Consecutive}/radian/";
            var nameFile = $"CUFE_{modelCufe.Consecutive}_{modelCufe.Nit}_{string.Format("{0:yyyyMMdd_HHmmss}", Utility.DateTimeCO)}.csv";
            var nameFileSucessFull = $"Exitoso_{nameFile}";
            var nameFileRejection = $"Rechazos_{nameFile}";
            var nameFileFal = $"FAL_{nameFile}";
            var resultCreateFile = await CreateFileCsvAsync(modelCufe, pathFile, nameFile);
            policyRetryFtpGetFile = GetPolicy(countRetryCountFtp, retrySecondExecution);
            var fileCreatedSucessFul = await CreateFileExcelFtpBlobStorageAsync(pathFile, nameFileSucessFull, modelCufe.OfferId);
            (bool resultProcess, int unprocessedRecord) resultFileSucessFulDian = (false, 0);
            if (fileCreatedSucessFul)
            {
                resultFileSucessFulDian = await ProcessFileSucessFulResponseDianAsync(modelCufe, pathFile, nameFileSucessFull);
                await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, modelCufe.OfferId, EventNotificationType.ProccesFileFtpTitle,
                GetErrorDescription(MessageCodes.ProcessFileFtp, nameFileSucessFull, totalCufe - resultFileSucessFulDian.unprocessedRecord, totalCufe, GetResultMessage(resultFileSucessFulDian.resultProcess)));
            }
            (bool resultProcess, int unprocessedRecord) resultRejectionDian = (false, 0);
            if (!fileCreatedSucessFul || resultFileSucessFulDian.unprocessedRecord > 0)
            {
                policyRetryFtpGetFile = GetPolicy(countRetryReject, retrySecondExecution);
                resultFileSucessFulDian.unprocessedRecord = resultFileSucessFulDian.unprocessedRecord == 0 ? totalCufe : resultFileSucessFulDian.unprocessedRecord;
                var fileCreatedRejection = await CreateFileExcelFtpBlobStorageAsync(pathFile, nameFileRejection, modelCufe.OfferId);
                resultRejectionDian = await ProcessFileRejectionResponseDianAsync(modelCufe, pathFile, nameFileRejection, fileCreatedRejection, resultFileSucessFulDian.unprocessedRecord);
                resultFileSucessFulDian.unprocessedRecord = 0;
                await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, modelCufe.OfferId, EventNotificationType.ProccesFileFtpTitle,
                GetErrorDescription(MessageCodes.ProcessFileFtp, nameFileRejection, totalCufe - resultRejectionDian.unprocessedRecord, totalCufe, GetResultMessage(resultRejectionDian.resultProcess)));
            }
            if (resultFileSucessFulDian.unprocessedRecord > 0 || resultRejectionDian.unprocessedRecord != 0)
            {
                policyRetryFtpGetFile = GetPolicy(countRetryFalled, retrySecondExecution);
                int unprocessedRecord = resultFileSucessFulDian.unprocessedRecord > 0 ? resultFileSucessFulDian.unprocessedRecord : resultRejectionDian.unprocessedRecord;
                var fileCreatedFal = await CreateFileExcelFtpBlobStorageAsync(pathFile, nameFileFal, modelCufe.OfferId);
                if (!fileCreatedFal)
                {
                    await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, modelCufe.OfferId, nameFile, messageNotFileFtp);
                    await UpdateStatusFaildResponseDianAsync(modelCufe.OfferId);
                    await SendEmail(modelCufe);
                    return false;
                }
                var resulFailed = await ProcessFileFailedResponseDianAsync(modelCufe, pathFile, nameFileFal, fileCreatedFal, unprocessedRecord);
                await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, modelCufe.OfferId, EventNotificationType.ProccesFileFtpTitle,
                GetErrorDescription(MessageCodes.ProcessFileFtp, nameFileFal, totalCufe - resulFailed.unprocessedRecord, totalCufe, GetResultMessage(resulFailed.resultProcess)));
                await UpdateStatusFaildResponseDianAsync(modelCufe.OfferId);
                await SendEmail(modelCufe);
                return true;
            }
            await UpdateStatusFaildResponseDianAsync(modelCufe.OfferId);
            await SendEmail(modelCufe);

            return resultCreateFile && (resultFileSucessFulDian.resultProcess || resultRejectionDian.resultProcess);
        }

        private async Task SendEmail(EnvoiceCufe modelCufe)
        {
            var existsStatusValidationDian = await invoiceRepository.ExistsStatusWaitValidationDianAsync(modelCufe.OfferId, CatalogCode_InvoiceStatus.WaitValidationDian);
            if (existsStatusValidationDian)
            {
                return;
            }

            var result = invoiceRepository.GetInvoicesProcessed(modelCufe.OfferId);

            #region Generar excel

            var dataTable = new DataTable();
            dataTable.Columns.Add("Factura", typeof(string));
            dataTable.Columns.Add("Estado", typeof(string));

            foreach (var item in result)
            {
                dataTable.Rows.Add(item.InvoiceNumber, item.status);
            }

            var docGenerated = ExcelBusiness.Generate(dataTable);

            #endregion Generar excel

            var template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToUserPostProcessDian);

            Dictionary<string, string> replacements = new()
        {
            { "{{offerNumber}}", modelCufe.Consecutive.ToString() },
            { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
        };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            EmainBusiness emainBusiness = new(_catalogBusiness);

            var email = await invoiceRepository.GetEmailUserByOffer(modelCufe.OfferId);
            await emainBusiness.SendAsync(email, Subject + modelCufe.Consecutive, templateWithData,
                new List<Common.Entities.AttachFile>() { new Common.Entities.AttachFile() { File = docGenerated, PathFileWithExtension = "FacturasProcesadas.xlsx" } });
        }

        private async Task<bool> CreateFileCsvAsync(EnvoiceCufe modelCufe, string pathFile, string nameFile)
        {
            var offerId = modelCufe?.OfferId ?? Guid.NewGuid();
            var createCsv = modelCufe?.Envoices.Select(s => new CufesCsv { Cufe = s.CUFE.Trim() }).ToList();
            var dataFileCsv = this.file.CreateFileCsv(createCsv);
            await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, offerId, EventNotificationType.CreatedFileTitle, GetErrorDescription(MessageCodes.FileDianGenerated, nameFile));
            var resultSeendFtp = await this.ftp.SendFileAsync(dataFileCsv, nameFile);
            await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, offerId, EventNotificationType.CreatedFileFtpTitle, GetErrorDescription(MessageCodes.SeedFileFtp, nameFile, GetResultMessage(resultSeendFtp)));
            var resultCreated = await this.storage.UploadAsync(dataFileCsv, pathFile + nameFile);
            await SaveEventNotificationAsync(EventNotificationType.CreatedFileBlobStorageId, offerId, EventNotificationType.CreatedFileBlobStorageTitle, GetErrorDescription(MessageCodes.FileDianCreatedBlobStorage, nameFile, pathFile));

            return resultCreated is not null && resultSeendFtp;
        }

        private async Task<bool> CreateFileExcelFtpBlobStorageAsync(string pathFile, string nameFile, Guid offerId)
        {
            var fileExcel = await GetFileFtpAsync(nameFile);
            if (fileExcel.Length <= 0)
            {
                await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, offerId, EventNotificationType.SearchFileFtpTitle,
                GetErrorDescription(MessageCodes.SearchFileFtp, nameFile, GetResultMessage(false)));
                return false;
            }
            var resultCreated = await this.storage.UploadAsync(fileExcel, pathFile + nameFile);
            bool result = resultCreated is not null;
            await SaveEventNotificationAsync(EventNotificationType.CreatedFileId, offerId, EventNotificationType.SearchFileFtpTitle,
               GetErrorDescription(MessageCodes.SearchFileFtp, nameFile, GetResultMessage(result)));
            return result;
        }

        private async Task<(bool resultProcess, int unprocessedRecord)> ProcessFileSucessFulResponseDianAsync(EnvoiceCufe envoiceCufe, string pathFile, string nameFile)
        {
            int totalCufeProcess = envoiceCufe.Envoices.Count;
            var dataFileCufeSuccesFul = await this.storage.DownloadByteAsync(pathFile + nameFile);
            if (dataFileCufeSuccesFul.Length <= 0)
            {
                return (resultProcess: false, unprocessedRecord: totalCufeProcess);
            }
            var dataCsvSucessFull = this.file.ReadFileCsv<SuccessFulCufes>(dataFileCufeSuccesFul);
            int totalSuccesFul = dataCsvSucessFull.Count;
            bool stateEvent032 = true;
            bool isClaimTrue = false;
            var invoiceEvent = dataCsvSucessFull.Select(s => new InvoiceEventInfo
            {
                Id = Guid.NewGuid(),
                InvoiceId = envoiceCufe?.Envoices?.First(c => c.CUFE.Trim() == s.CUFE.Trim())?.EnvoiceId ?? Guid.Empty,
                Event030 = GetEvent(s.AcuseRecibo),
                Event032 = GetEvent(s.FechaEntregaProductoServicioValidoNegociar) ? stateEvent032 : s.FechaEntregaProductoServicio?.Trim().Length >= 9,
                Event033 = GetEvent(s.Reclamo) ? isClaimTrue : GetEvent(s.AceptacionExpresa) || GetEvent(s.AceptacionTacita),
                Event036 = GetEvent(s.InscritaFEcomoTV),
                Event037 = GetEvent(s.Endoso),
                Event06 = GetEvent(s.TieneEventoPago),
                Event07 = GetEvent(s.TieneEventoPago),
                Claim = GetEvent(s.Reclamo),
                Message = GetMessageRejection(new InvoiceDataGetState { Event036 = GetEvent(s.InscritaFEcomoTV), Event037 = GetEvent(s.Endoso), Event06 = GetEvent(s.TieneEventoPago), Event07 = GetEvent(s.TieneEventoPago), Reclamo = GetEvent(s.Reclamo) })
            }).ToList();
            var stateInvoiceEvent = GetDataStateInvoice(invoiceEvent, envoiceCufe.Envoices, dataCsvSucessFull, new List<RejectionCufe>());
            bool resultState = await SaveStateAsync(invoiceEvent, stateInvoiceEvent);
            return (resultProcess: invoiceEvent.Count > 0 && resultState, unprocessedRecord: totalCufeProcess - totalSuccesFul);
        }

        private async Task<(bool resultProcess, int unprocessedRecord)> ProcessFileRejectionResponseDianAsync(EnvoiceCufe envoiceCufe, string pathFile, string nameFile,
            bool fileIsCreated, int totalCufeProcess)
        {
            if (!fileIsCreated)
            {
                return (resultProcess: false, unprocessedRecord: totalCufeProcess);
            }
            var dataFileCufeRejection = await this.storage.DownloadByteAsync(pathFile + nameFile);
            if (dataFileCufeRejection.Length <= 0)
            {
                return (resultProcess: false, unprocessedRecord: totalCufeProcess);
            }
            var dataCsvRejection = this.file.ReadFileCsv<RejectionCufe>(dataFileCufeRejection);
            int totalRejection = dataCsvRejection.Count;
            bool stateEvent032 = true;
            bool isClaimTrue = false;
            var invoiceEventRejection = dataCsvRejection.Select(s => new InvoiceEventInfo
            {
                Id = Guid.NewGuid(),
                InvoiceId = envoiceCufe?.Envoices?.First(c => c.CUFE.Trim() == s.CUFE.Trim())?.EnvoiceId ?? Guid.Empty,
                Event030 = GetEvent(s.AcuseRecibo),
                Event032 = GetEvent(s.FechaEntregaProductoServicioValidoNegociar) ? stateEvent032 : s.FechaEntregaProductoServicio?.Trim().Length >= 9,
                Event033 = GetEvent(s.Reclamo) ? isClaimTrue : GetEvent(s.AceptacionExpresa) || GetEvent(s.AceptacionTacita),
                Event036 = GetEvent(s.InscritaFEcomoTV),
                Event037 = GetEvent(s.Endoso),
                Event06 = GetEvent(s.TieneEventoPago),
                Event07 = GetEvent(s.TieneEventoPago),
                Claim = GetEvent(s.Reclamo),
                Message = GetMessageRejection(new InvoiceDataGetState { Event036 = GetEvent(s.InscritaFEcomoTV), Event037 = GetEvent(s.Endoso), Event06 = GetEvent(s.TieneEventoPago), Event07 = GetEvent(s.TieneEventoPago), Reclamo = GetEvent(s.Reclamo) })
            }).ToList();
            var stateInvoiceEvent = GetDataStateInvoice(invoiceEventRejection, envoiceCufe.Envoices, new List<SuccessFulCufes>(), dataCsvRejection);
            bool resultState = await SaveStateAsync(invoiceEventRejection, stateInvoiceEvent);

            return (resultProcess: invoiceEventRejection.Count > 0 && resultState, unprocessedRecord: totalCufeProcess - totalRejection);
        }

        private async Task<(bool resultProcess, int unprocessedRecord)> ProcessFileFailedResponseDianAsync(EnvoiceCufe envoiceCufe, string pathFile,
            string nameFile, bool fileIsCreated, int totalCufeProcess)
        {
            if (!fileIsCreated)
            {
                return (resultProcess: false, unprocessedRecord: totalCufeProcess);
            }
            var dataFileCufeRejection = await this.storage.DownloadByteAsync(pathFile + nameFile);
            var dataCsvFailed = this.file.ReadFileCsv<FailedCufe>(dataFileCufeRejection);
            int totalFailed = dataCsvFailed.Count;
            var result = await SaveStateFailedInvoicesAsync(envoiceCufe.Envoices, dataCsvFailed);
            return (resultProcess: result, unprocessedRecord: totalCufeProcess - totalFailed);
        }

        private async Task<bool> SaveEventNotificationAsync(Guid typeEnvent, Guid offerId, string to, string body)
        {
            var dataEvent = new EventNotificationInfo { Id = Guid.NewGuid(), TypeId = typeEnvent, OfferId = offerId, To = to, Body = body };
            var result = await this.eventNotificationRepository.AddEventNotificationAsync(dataEvent);
            await this.unitOfWork.SaveChangesAsync();
            return result;
        }

        private static bool GetEvent(string data)
        {
            bool eventSucessFull;
            if (bool.TryParse(data, out eventSucessFull))
            {
                return eventSucessFull;
            }
            return eventSucessFull;
        }

        private async Task<byte[]> GetFileFtpAsync(string nameFile)
        {
            if (policyRetryFtpGetFile is null)
            {
                return new byte[0];
            }
            var resultFtp = await policyRetryFtpGetFile.ExecuteAsync(async () =>
            {
                bool isDeleteFile = true;
                var fileSucessFull = await this.ftp.GetFileAsync(nameFile, isDeleteFile);
                if (fileSucessFull is null || fileSucessFull.Length <= 0)
                {
                    this.logger.LogInformation($"NO EXISTE EL ARCHIVO EN EL FTPS :{nameFile}");
                }
                return fileSucessFull ?? new byte[0];
            });
            return resultFtp;
        }

        private async Task<bool> SaveStateAsync(List<InvoiceEventInfo> invoiceEvents, List<InvoiceInfo> stateInvoiceEvent)
        {
            if (!invoiceEvents.Any() && !stateInvoiceEvent.Any())
            {
                return false;
            }
            await this.invoiceRepository.UpdateStateInvoiceAsync(stateInvoiceEvent);
            await this.invoiceEventRepository.AddInvoiceEventAsync(invoiceEvents);
            var result = await this.unitOfWork.SaveChangesAsync();
            return result >= 0;
        }

        private async Task<bool> SaveStateFailedInvoicesAsync(List<Envoice> invoices, List<FailedCufe> failedCufes)
        {
            if (!failedCufes.Any())
            {
                return false;
            }
            var dataFailedInvoice = (from IN in invoices
                                     join FA in failedCufes on IN.CUFE.Trim() equals FA.CUFE.Trim()
                                     select new InvoiceInfo { Id = IN.EnvoiceId, StatusId = StateInvoice.Rejection, ErrorMessage = FA.CodigoError }).ToList();
            var result = await this.invoiceRepository.UpdateStateInvoiceAsync(dataFailedInvoice);
            return result;
        }

        private List<InvoiceInfo> GetDataStateInvoice(List<InvoiceEventInfo> invoiceEvents, List<Envoice> invoices, List<SuccessFulCufes> successFulCufes, List<RejectionCufe> rejectionCufes)
        {
            var invoiceData = successFulCufes.Any() ?
                             (from IN in invoices
                              join EV in invoiceEvents on IN.EnvoiceId equals EV.InvoiceId
                              select new { IN.CUFE, EV.InvoiceId, EV.Event030, EV.Event032, EV.Event033, EV.Event036, EV.Event037, EV.Event06, EV.Event07, EV.Claim } into M
                              join SC in successFulCufes on M.CUFE.Trim() equals SC.CUFE.Trim()
                              select new InvoiceDataGetState
                              {
                                  InvoiceId = M.InvoiceId,
                                  Endoso = GetEvent(SC.Endoso),
                                  Tiene_Evento_Pago = GetEvent(SC.TieneEventoPago),
                                  Event030 = M.Event030,
                                  Event032 = M.Event032,
                                  Event033 = M.Event033,
                                  Event036 = M.Event036,
                                  Event037 = M.Event037,
                                  Event06 = M.Event06 ?? false,
                                  Event07 = M.Event07 ?? false,
                                  Reclamo = M.Claim ?? false,
                              }).ToList()
                              :
                            (from IN in invoices
                             join EV in invoiceEvents on IN.EnvoiceId equals EV.InvoiceId
                             select new { IN.CUFE, EV.InvoiceId, EV.Event030, EV.Event032, EV.Event033, EV.Event036, EV.Event037, EV.Event06, EV.Event07, EV.Claim } into M
                             join RE in rejectionCufes on M.CUFE.Trim() equals RE.CUFE.Trim()
                             select new InvoiceDataGetState
                             {
                                 InvoiceId = M.InvoiceId,
                                 Endoso = GetEvent(RE.Endoso),
                                 Tiene_Evento_Pago = GetEvent(RE.TieneEventoPago),
                                 Event030 = M.Event030,
                                 Event032 = M.Event032,
                                 Event033 = M.Event033,
                                 Event036 = M.Event036,
                                 Event037 = M.Event037,
                                 Event06 = M.Event06 ?? false,
                                 Event07 = M.Event07 ?? false,
                                 Reclamo = M.Claim ?? false,
                             }).ToList();

            var invoiceIds = invoiceData.Select(s => new InvoiceInfo { Id = s.InvoiceId, StatusId = GetStateInvoice(s) }).ToList();

            return invoiceIds;
        }

        private string GetMessageRejection(InvoiceDataGetState invoiceData)
        {
            var message = string.Empty;
            var space = " ";
            if (invoiceData.Reclamo)
            {
                message += GetErrorDescription(MessageCodes.MessegeEventDian031, space);
            }
            if (invoiceData.Event037)
            {
                message += GetErrorDescription(MessageCodes.MessegeWhitEventRadian037, space);
                message += GetErrorDescription(MessageCodes.MessegeWithoutEventRadian037, space);
            }
            if (invoiceData.Event06)
            {
                message += GetErrorDescription(MessageCodes.MessegeRadian046, space);
            }
            if (invoiceData.Event07)
            {
                message += GetErrorDescription(MessageCodes.MessegeRadian045, space);
            }
            return message;
        }

        private Guid GetStateInvoice(InvoiceDataGetState invoiceData)
        {
            if (invoiceData.Event037 || invoiceData.Event06 || invoiceData.Event07 || invoiceData.Reclamo)
            {
                return StateInvoice.Rejection;
            }

            return StateInvoice.Approved;
        }

        private string GetResultMessage(bool result) => result ? EventNotificationType.MessageSuccess : EventNotificationType.MessageFailed;

        private AsyncRetryPolicy<byte[]> GetPolicy(int retryCount, int timeSecoundRentry)
        {
            return Policy<byte[]>.Handle<Exception>()
                                                     .OrResult(r => r.Length <= 0)
                                                     .WaitAndRetryAsync(retryCount: retryCount,
                                                      retry => TimeSpan.FromSeconds(timeSecoundRentry * retry),
                                                      onRetry: (exception, time, contexto) =>
                                                      {
                                                          Console.WriteLine($"INTERVALO TIEMPO BÚSQUEDA ARCHIVO FTPS :{time}");
                                                      });
        }

        private async Task UpdateStatusFaildResponseDianAsync(Guid offerId)
        {
            await this.invoiceRepository.UpdateStateInvoiceProcessDianAsync(offerId);
        }
    }
}