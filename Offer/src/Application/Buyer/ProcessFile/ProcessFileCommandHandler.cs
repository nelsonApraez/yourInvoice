///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Buyer.EmailToAdmin;
using yourInvoice.Offer.Application.Buyer.EmailToBuyer;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;
using yourInvoice.Offer.Domain.OperationFiles;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Text;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.ProcessFile
{
    public sealed class ProcessFileCommandHandler : IRequestHandler<ProcessFileCommand, ErrorOr<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileOperation fileOperation;
        private readonly IStorage storage;
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly IOperationFileRepository operationFileRepository;
        private readonly IOfferRepository offerRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IEventNotificationsRepository eventNotificationsRepository;
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IMediator mediator;
        private readonly IUserRepository userRepository;
        private const string titleNotification = "Oferta pendiente compra archivo ftp factoring";
        private const string titleEventNotification = "Archivo procesado Ftp Factoring";
        private const string pathFile = "factoring/received/";
        private const string canceledRegistration = "A";
        private const string previouslyModifiedRecord = "M";
        private const string newModifiedRecord = "R";
        private const string newRecords = "N";
        private int purchaseHoursValidity = 24;
        private const string containerFileWithError = "yourInvoicecontainer";
        private const int repeatTransactions = 1;

        public ProcessFileCommandHandler(IUnitOfWork unitOfWork, IFileOperation fileOperation, IStorage storage, IInvoiceDispersionRepository invoiceDispersionRepository,
            IOperationFileRepository operationFileRepository, IOfferRepository offerRepository, INotificationRepository notificationRepository,
            IEventNotificationsRepository eventNotificationsRepository, ICatalogBusiness catalogBusiness, IMediator mediator, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.fileOperation = fileOperation;
            this.storage = storage;
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.operationFileRepository = operationFileRepository;
            this.offerRepository = offerRepository;
            this.notificationRepository = notificationRepository;
            this.eventNotificationsRepository = eventNotificationsRepository;
            this.catalogBusiness = catalogBusiness;
            this.mediator = mediator;
            this.userRepository = userRepository;
        }

        public async Task<ErrorOr<bool>> Handle(ProcessFileCommand command, CancellationToken cancellationToken)
        {
            var purchaseTimeValidity = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableOffers);
            int.TryParse(purchaseTimeValidity?.Descripton, out purchaseHoursValidity);
            var filesWithoutProcces = await this.operationFileRepository.GetFilesWithoutProcessAsync();
            if (!filesWithoutProcces.Any())
            {
                return false;
            }
            await this.operationFileRepository.UpdateStartDateAsync(filesWithoutProcces);
            var notification = new List<Notification>();
            var eventNotification = new List<EventNotification>();
            var create = Guid.NewGuid();

            foreach (var name in filesWithoutProcces.Select(s => s.Name))
            {
                var nameFile = name;
                var offerNumber = GetOfferNumber(nameFile);
                var offerId = await this.offerRepository.GetIdOfferAsync(offerNumber);
                var dataFileExcel = await GetDataFileExcelAsync(nameFile);
                if (dataFileExcel is null || dataFileExcel.Count <= 0)
                {
                    filesWithoutProcces.Where(c => c.Name == nameFile).ToList().ForEach(f => { f.Description = GetErrorDescription(MessageCodes.FtpFactoringFileNotExists, nameFile, pathFile); });
                    await this.operationFileRepository.UpdateStateToProcessAsync(filesWithoutProcces);
                    await this.unitOfWork.SaveChangesAsync(cancellationToken);
                    continue;
                }
                (var dataValidated, var message) = await ProcessValidateDataAsync(dataFileExcel, offerNumber);
                if (!string.IsNullOrEmpty(message))
                {
                    var expirationLinkOneMonth = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableLink);
                    var linkFile = await this.storage.GenerateSecureDownloadUrlAsync(pathFile + nameFile, containerFileWithError, tiempoExpiracionMinutos: Convert.ToInt32(expirationLinkOneMonth.Descripton.Trim()));
                    await this.mediator.Publish(new EmailToAdminProcessFileCommand { NameFile = name, LinkFile = linkFile, MessageValidationFile = message });
                }
                await ProcessCanceledRegistrationAsync(dataValidated, offerNumber, cancellationToken);
                await ProcessPreviouslyModifiedRecordAsync(dataValidated, offerNumber, cancellationToken);
                await ProcessNewModifiedRecordAsync(dataValidated, offerNumber, cancellationToken);
                await ProcessNewRecordsIAsync(dataValidated, offerNumber, cancellationToken);
                filesWithoutProcces.Where(c => c.Name == nameFile).ToList().ForEach(f => { f.Description = message; });
                notification.Add(GetNotification(create, offerNumber));
                var IsNotExistsOffer = offerId == Guid.Empty;
                if (!IsNotExistsOffer)
                {
                    eventNotification.Add(GetEventNotification(create, offerId, nameFile));
                }
                if (string.IsNullOrEmpty(message))
                {
                    var BuyerIds = dataFileExcel.Select(x => x.BuyerId).Distinct();
                    foreach (var itemId in BuyerIds)
                    {
                        var userBuyer = await userRepository.GetByIdAsync(itemId);
                        await this.mediator.Publish(new EmailToBuyerNewOffersCommand { NumberOffer = offerNumber, EmailBuyerNotification = userBuyer.Email });
                    }
                }
            }
            await this.notificationRepository.AddAsync(notification);
            await this.eventNotificationsRepository.AddAsync(eventNotification);
            await this.operationFileRepository.UpdateStateToProcessAsync(filesWithoutProcces);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<List<PurchaseOperation>> GetDataFileExcelAsync(string nameFileExcelStorage)
        {
            var dataFile = await this.storage.DownloadByteAsync(pathFile + nameFileExcelStorage);
            if (dataFile.Length <= 0)
            {
                return new List<PurchaseOperation>();
            }
            var dataExcel = this.fileOperation.ReadFileExcel<PurchaseOperation>(dataFile);
            return dataExcel;
        }

        private int GetOfferNumber(string nameFile)
        {
            if (string.IsNullOrEmpty(nameFile))
            {
                return 0;
            }
            var offerNumber = nameFile.Split('-')[1].Split('_')[0];
            int number = 0;
            int.TryParse(offerNumber, out number);
            return number;
        }

        private async Task<(List<PurchaseOperation>, string messageValidation)> ProcessValidateDataAsync(IEnumerable<PurchaseOperation> purchases, int numberOffer)
        {
            var dataValidated = new List<PurchaseOperation>();
            purchases.ToList().ForEach(f => f.FechaDeCompra = ExtensionFormat.DateTimeCO());
            var message = new StringBuilder();
            bool isNotReasignation = purchases.Any(c =>
               c.Reasignacion.ToUpperInvariant() == canceledRegistration || c.Reasignacion.ToUpperInvariant() == previouslyModifiedRecord
            || c.Reasignacion.ToUpperInvariant() == newModifiedRecord || c.Reasignacion.ToUpperInvariant() == newRecords);
            if (!isNotReasignation)
            {
                message.Append(GetErrorDescription(MessageCodes.FactoringReasignation) + " ");
                return (dataValidated, messageValidation: message.ToString());
            }
            int cnRegister = 1;
            foreach (var p in purchases)
            {
                cnRegister++;
                var messageEmpty = ValidateFieldEmpty(p, cnRegister);
                if (!string.IsNullOrEmpty(messageEmpty))
                {
                    message.AppendLine(messageEmpty);
                    continue;
                }
                var nroInvoiceOnlyNumber = RemoveDispersionInvoice(p.FacturaNo);
                if (string.IsNullOrEmpty(nroInvoiceOnlyNumber))
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringNroInvoiceInvalid, p.FacturaNo));
                    continue;
                }
                var dataValidation = await this.offerRepository.GetDataValidateStateOfferEnabledAsync(numberOffer, nroInvoiceOnlyNumber, p.NumeroDeDocumentoPagador);
                if (dataValidation == null || !dataValidation.PayerNit.Equals(p.NumeroDeDocumentoPagador))
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringNoExistsPayer, p.FacturaNo, numberOffer, p.NumeroDeDocumentoPagador) + " ");
                    continue;
                }
                var countTransaction = await this.invoiceDispersionRepository.CountTransactionAsync(numberOffer, p.NoTran, p.NumeroDeDocumentoComprador);
                DataValidationProcessFileResponse invoiceDispersionId;
                if (countTransaction > repeatTransactions)
                {
                    invoiceDispersionId = await this.invoiceDispersionRepository.ValidateExistsInvoiceAsync(numberOffer, p.FacturaNo, p.NumeroDeDocumentoComprador);
                }
                else
                {
                    invoiceDispersionId = await this.invoiceDispersionRepository.ValidateExistsAsync(numberOffer, p.NoTran, p.NumeroDeDocumentoComprador);
                }
                var IsNotExistsInvoice = invoiceDispersionId?.InvoiceDispersionId == Guid.Empty;
                if (p.Reasignacion != newModifiedRecord && !IsNotExistsInvoice && invoiceDispersionId?.InvoiceDispersionStatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringNoExistsInvoice, p.FacturaNo, numberOffer) + " ");
                    continue;
                }
                var sellerId = await this.invoiceDispersionRepository.ValidateExistsSellerAsync(numberOffer, p.NumeroDeDocumentoVendedor);
                var sellerNoExist = sellerId == Guid.Empty;
                if (sellerNoExist)
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringNoExistsSeller, p.FacturaNo, numberOffer, p.NumeroDeDocumentoVendedor) + " ");
                    continue;
                }
                if (p.FechaFinal.Date < p.FechaDeCompra.Date)
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringHighestPurchaseDateFinalDate, p.FechaDeCompra.Date, p.FechaFinal) + " ");
                    continue;
                }
                int smallestWeek = -7;
                if (p.FechaFinal.AddDays(smallestWeek).Date < ExtensionFormat.DateTimeCO().Date)
                {
                    message.Append(GetErrorDescription(MessageCodes.FtpFactoringPurchaseDateMinorWeek, p.FechaFinal.AddDays(smallestWeek).Date) + " ");
                    continue;
                }
                var userBuyer = await this.userRepository.GetUserAsync(p.NumeroDeDocumentoComprador, CatalogCode_UserRole.Buyer);
                if (userBuyer is null || userBuyer.DocumentNumber.Length <= 0)
                {
                    message.Append(GetErrorDescription(MessageCodes.BuyerNotExist, p.NumeroDeDocumentoComprador) + " ");
                    continue;
                }
                p.PayerId = dataValidation.PayerId;
                p.BuyerId = userBuyer.Id;
                p.SellerId = sellerId;
                p.InvoiceDispersionId = invoiceDispersionId?.InvoiceDispersionId ?? Guid.Empty;
                dataValidated.Add(p);
            }
            return (dataValidated, messageValidation: message.ToString());
        }

        private async Task<bool> ProcessCanceledRegistrationAsync(List<PurchaseOperation> purchases, int offerNumber, CancellationToken cancellationToken)
        {
            var dataNewPurchate = purchases?.Where(c => c.Reasignacion.ToUpperInvariant() == canceledRegistration);
            if (!dataNewPurchate.Any())
            {
                return false;
            }
            List<InvoiceDispersion> invoicesDispersion = new();
            dataNewPurchate.ToList().ForEach(p => { invoicesDispersion.Add(GetInvoiceDispersion(p, offerNumber, CatalogCode_InvoiceDispersionStatus.Canceled, canceledRegistration, false)); });
            await this.invoiceDispersionRepository.UpdateReassignmentAsync(invoicesDispersion);
            await AddHistorical(offerNumber, invoicesDispersion, cancellationToken);
            return true;
        }

        private async Task<bool> ProcessPreviouslyModifiedRecordAsync(List<PurchaseOperation> purchases, int offerNumber, CancellationToken cancellationToken)
        {
            var dataNewPurchate = purchases?.Where(c => c.Reasignacion.ToUpperInvariant() == previouslyModifiedRecord);
            if (!dataNewPurchate.Any())
            {
                return false;
            }
            List<InvoiceDispersion> invoicesDispersion = new();
            dataNewPurchate.ToList().ForEach(p => { invoicesDispersion.Add(GetInvoiceDispersion(p, offerNumber, CatalogCode_InvoiceDispersionStatus.CanceledModified, previouslyModifiedRecord, false)); });
            await this.invoiceDispersionRepository.UpdateReassignmentAsync(invoicesDispersion);
            await AddHistorical(offerNumber, invoicesDispersion, cancellationToken);
            return true;
        }

        private async Task<bool> ProcessNewModifiedRecordAsync(List<PurchaseOperation> purchases, int offerNumber, CancellationToken cancellationToken)
        {
            var dataNewPurchate = purchases?.Where(c => c.Reasignacion.ToUpperInvariant() == newModifiedRecord);
            if (!dataNewPurchate.Any())
            {
                return false;
            }
            List<InvoiceDispersion> invoicesDispersion = new();
            dataNewPurchate.Where(c => c.InvoiceDispersionId != Guid.Empty).ToList().ForEach(p => { invoicesDispersion.Add(GetInvoiceDispersion(p, offerNumber, CatalogCode_InvoiceDispersionStatus.PendingPurchase, newModifiedRecord)); });
            if (invoicesDispersion?.Count() > 0)
            {
                await this.invoiceDispersionRepository.UpdateReassignmentAsync(invoicesDispersion);
            }
            invoicesDispersion = new();
            dataNewPurchate.Where(c => c.InvoiceDispersionId == Guid.Empty && c.Reasignacion == newModifiedRecord).ToList().ForEach(p => { invoicesDispersion.Add(GetInvoiceDispersion(p, offerNumber, CatalogCode_InvoiceDispersionStatus.PendingPurchase, newModifiedRecord)); });
            if (invoicesDispersion?.Count() > 0)
            {
                await this.invoiceDispersionRepository.AddAsync(invoicesDispersion);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await AddHistorical(offerNumber, invoicesDispersion, cancellationToken);
            }
            return true;
        }

        private async Task<bool> ProcessNewRecordsIAsync(List<PurchaseOperation> purchases, int offerNumber, CancellationToken cancellationToken)
        {
            var dataNewPurchate = purchases?.Where(c => c.Reasignacion.ToUpperInvariant() == newRecords);
            if (!dataNewPurchate.Any())
            {
                return false;
            }
            List<InvoiceDispersion> invoicesDispersion = new();
            dataNewPurchate.ToList().ForEach(p => { invoicesDispersion.Add(GetInvoiceDispersion(p, offerNumber, CatalogCode_InvoiceDispersionStatus.PendingPurchase, newRecords)); });
            var result = await this.invoiceDispersionRepository.AddAsync(invoicesDispersion);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            await AddHistorical(offerNumber, invoicesDispersion, cancellationToken);
            return result;
        }

        private InvoiceDispersion GetInvoiceDispersion(PurchaseOperation dataOperation, int offerNumber, Guid statusId, string reasignacion, bool status = true)
        {
            int numberReminder = 0;
            var IsNotExistsInvoice = dataOperation.InvoiceDispersionId == Guid.Empty;
            dataOperation.InvoiceDispersionId = IsNotExistsInvoice && dataOperation.Reasignacion != newModifiedRecord ? Guid.NewGuid() : dataOperation.InvoiceDispersionId;
            return new InvoiceDispersion(dataOperation.InvoiceDispersionId, offerNumber, dataOperation.BuyerId, dataOperation.SellerId, dataOperation.PayerId, dataOperation.FechaDeCompra,
                dataOperation.FechaFinal, dataOperation.NoTran, dataOperation.FacturaNo, dataOperation.Fraccionamiento, dataOperation.TasaEAPorcentaje, dataOperation.DiasOper, dataOperation.VrCompra,
                dataOperation.VrFuturo, Convert.ToChar(reasignacion), dataOperation.DineroNuevo == "SI", statusId,
                ExtensionFormat.DateTimeCO().AddHours(purchaseHoursValidity), numberReminder, DateTime.MinValue, status, dataOperation.FechaOperacion, dataOperation.FechaEsperada, dataOperation.TransaccionPadre
                , ExtensionFormat.DateTimeCO(), dataOperation.PayerId);
        }

        private static Notification GetNotification(Guid create, int offerNumber)
        {
            return new Notification(Guid.NewGuid(), titleNotification, GetErrorDescription(MessageCodes.FtpFactoringOfferProcessBuyer, offerNumber),
                false, ExtensionFormat.DateTimeCO(), create, ExtensionFormat.DateTimeCO(), create);
        }

        private static EventNotification GetEventNotification(Guid create, Guid offerId, string nameFile)
        {
            return new EventNotification(Guid.NewGuid(), offerId, Guid.NewGuid(), GetErrorDescription(MessageCodes.FtpFactoringFileProcess, nameFile), titleEventNotification,
                true, ExtensionFormat.DateTimeCO(), create, ExtensionFormat.DateTimeCO(), create);
        }

        private static string RemoveDispersionInvoice(string nroInvoice)
        {
            if (string.IsNullOrEmpty(nroInvoice))
            {
                return string.Empty;
            }
            var invoice = nroInvoice.Contains(@"/") ? nroInvoice.Substring(0, nroInvoice.LastIndexOf(@"/")) : nroInvoice;
            return invoice;
        }

        private async Task AddHistorical(int offerNumber, List<InvoiceDispersion> invoicesDispersion, CancellationToken cancellationToken)
        {
            await this.mediator.Publish(new AddHistoricalCommand
            {
                NumberOffer = offerNumber,
                PayerId = invoicesDispersion?.FirstOrDefault()?.PayerId,
                InvoiceDispersionId = invoicesDispersion?.Select(s => s.Id).ToList(),
                StatusId = CatalogCode_InvoiceDispersionStatus.Canceled,
                UserId = invoicesDispersion?.FirstOrDefault()?.BuyerId
            }, cancellationToken);
        }

        private static string ValidateFieldEmpty(PurchaseOperation purchase, int cnRegister)
        {
            var fieldEmpty = new List<string> { "TransaccionPadre" };
            var purchases = purchase.GetType().GetProperties();
            var messageValidation = new StringBuilder();
            foreach (var propertiePurchase in purchases)
            {
                var dataValue = Convert.ToString(propertiePurchase.GetValue(purchase));
                bool isNotValidateEmpty = fieldEmpty.Exists(a => a.Equals(propertiePurchase.Name));
                if (string.IsNullOrEmpty(dataValue) && !isNotValidateEmpty)
                {
                    messageValidation.Append(GetErrorDescription(MessageCodes.FactoringFieldEmpty, cnRegister, propertiePurchase.Name) + " ");
                }
            }
            return Convert.ToString(messageValidation);
        }
    }
}