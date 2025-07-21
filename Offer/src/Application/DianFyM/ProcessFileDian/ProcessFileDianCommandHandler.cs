///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.DianFyM.Email;
using yourInvoice.Offer.Domain.DianFyMFiles;
using yourInvoice.Offer.Domain.DianFyMFiles.Queries;
using yourInvoice.Offer.Domain.Notifications;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.DianFyM.ProcessFileDian
{
    public sealed class ProcessFileDianCommandHandler : IRequestHandler<ProcessFileDianCommand, ErrorOr<bool>>
    {
        private readonly IDianFyMFileRepository dianFyMFileRepository;
        private readonly IStorage storage;
        private readonly IFileOperation file;
        private readonly IUnitOfWorkCommonEF unitOfWork;
        private readonly IInvoiceEventRepository invoiceEventRepository;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IMediator mediator;
        private const string typeResponseFile = "FAL_";

        public ProcessFileDianCommandHandler(IDianFyMFileRepository dianFyMFileRepository, IStorage storage, IFileOperation file,
             INotificationRepository notificationRepository, IUnitOfWorkCommonEF unitOfWork, IInvoiceEventRepository invoiceEventRepository,
            IInvoiceRepository invoiceRepository, IMediator mediator)
        {
            this.dianFyMFileRepository = dianFyMFileRepository;
            this.storage = storage;
            this.file = file;
            this.unitOfWork = unitOfWork;
            this.invoiceEventRepository = invoiceEventRepository;
            this.invoiceRepository = invoiceRepository;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(ProcessFileDianCommand command, CancellationToken cancellationToken)
        {
            var nameFilesNoProcess = await this.dianFyMFileRepository.GetDianNemeFilesNoProcessAsync();
            if (nameFilesNoProcess is null || !nameFilesNoProcess.Any())
            {
                return false;
            }
            await ValidationProcessDianAsync(nameFilesNoProcess);
            return true;
        }

        private async Task ValidationProcessDianAsync(IEnumerable<DianFyMFile> files)
        {
            await this.dianFyMFileRepository.UpdateStateToProcessAsync(files);
            foreach (var dataFile in files)
            {
                var invoices = await this.dianFyMFileRepository.GetInvoiceAsync(dataFile.Offer, CatalogCode_InvoiceStatus.WaitValidationDian);
                if (dataFile.Name.Trim().ToUpperInvariant().Contains(typeResponseFile))
                {
                    await ProcessFileFailedResponseDianAsync(invoices.ToList(), dataFile.PathStorage, dataFile.Name, dataFile.Id);
                    continue;
                }
                await ProcessFileResponseDianAsync(dataFile.PathStorage, dataFile.Name, invoices, dataFile.Id);
            }
            await SendEmailPostProcessDianAsync(files);
        }

        private async Task<bool> ProcessFileResponseDianAsync(string pathFile, string nameFile, IEnumerable<InvoiceCufeDian> invoices, Guid dianFymId)
        {
            var dataFileCufeRejection = await this.storage.DownloadByteAsync(pathFile + nameFile);
            var dataCsv = this.file.ReadFileCsv<ResponseFileDian>(dataFileCufeRejection);
            bool stateEvent032 = true;
            bool isClaimTrue = false;
            var invoiceEvent = dataCsv.Select(s => new InvoiceEventInfo
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoices?.First(c => c.CUFE.Trim() == s.CUFE.Trim())?.EnvoiceId ?? Guid.Empty,
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
            var stateInvoiceEvent = GetDataStateInvoice(invoiceEvent, invoices?.ToList(), dataCsv);
            await SaveStateAsync(invoiceEvent, stateInvoiceEvent);
            await this.dianFyMFileRepository.UpdateCountRegisterAsync(dianFymId, stateInvoiceEvent.Count);
            return true;
        }

        private List<InvoiceInfo> GetDataStateInvoice(List<InvoiceEventInfo> invoiceEvents, List<InvoiceCufeDian> invoices, List<ResponseFileDian> cufes)
        {
            var invoiceData = (from IN in invoices
                               join EV in invoiceEvents on IN.EnvoiceId equals EV.InvoiceId
                               select new { IN.CUFE, EV.InvoiceId, EV.Event030, EV.Event032, EV.Event033, EV.Event036, EV.Event037, EV.Event06, EV.Event07, EV.Claim } into M
                               join SC in cufes on M.CUFE.Trim() equals SC.CUFE.Trim()
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
                               }).ToList();

            var invoiceIds = invoiceData.Select(s => new InvoiceInfo { Id = s.InvoiceId, StatusId = GetStateInvoice(s) }).ToList();

            return invoiceIds;
        }

        private async Task<bool> ProcessFileFailedResponseDianAsync(List<InvoiceCufeDian> invoices, string pathFile, string nameFile, Guid dianFymId)
        {
            var dataFileCufeRejection = await this.storage.DownloadByteAsync(pathFile + nameFile);
            var dataCsvFailed = this.file.ReadFileCsv<FailedCufe>(dataFileCufeRejection);
            await SaveStateFailedInvoicesAsync(invoices, dataCsvFailed);
            await this.dianFyMFileRepository.UpdateCountRegisterAsync(dianFymId, dataCsvFailed.Count);
            return true;
        }

        private async Task<bool> SaveStateFailedInvoicesAsync(List<InvoiceCufeDian> invoices, List<FailedCufe> failedCufes)
        {
            if (!failedCufes.Any())
            {
                return false;
            }
            var dataFailedInvoice = (from IN in invoices
                                     join FA in failedCufes on IN.CUFE.Trim() equals FA.CUFE.Trim()
                                     select new InvoiceInfo { Id = IN.EnvoiceId, StatusId = CatalogCode_InvoiceStatus.Rejected, ErrorMessage = FA.CodigoError }).ToList();
            var result = await this.invoiceRepository.UpdateStateInvoiceAsync(dataFailedInvoice);
            return result;
        }

        private async Task<bool> SaveStateAsync(List<InvoiceEventInfo> invoiceEvents, List<InvoiceInfo> stateInvoiceEvent)
        {
            if (!invoiceEvents.Any() && !stateInvoiceEvent.Any())
            {
                return false;
            }
            await this.invoiceRepository.UpdateStateInvoiceAsync(stateInvoiceEvent);
            await this.invoiceEventRepository.AddInvoiceEventAsync(invoiceEvents);
            await this.unitOfWork.SaveChangesAsync();
            return true;
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
                return CatalogCode_InvoiceStatus.Rejected;
            }

            return CatalogCode_InvoiceStatus.Approved;
        }

        private async Task SendEmailPostProcessDianAsync(IEnumerable<DianFyMFile> files)
        {
            var offers = files.Select(s => s.Offer).Distinct().ToList();
            foreach (var offer in offers)
            {
                await this.mediator.Publish(new EmailToSellerPostProcessDianCommand { Consecutive = offer, OfferId = Guid.Empty });
            }
        }
    }
}