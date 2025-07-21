///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Buyer.EmailToAdmin;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.Reject
{
    public sealed class RejectCommandHandler : IRequestHandler<RejectCommand, ErrorOr<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStorage storage;
        private readonly IFileOperation fileOperation;
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly IMediator mediator;
        private readonly ISystem system;
        private readonly IOfferRepository _offerRepository;
        private const string pathFileStorage = "/storage/FacturasRechazadas/";

        public RejectCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IFileOperation fileOperation, ICatalogBusiness catalogBusiness,
            IInvoiceDispersionRepository invoiceDispersionRepository, IMediator mediator, ISystem system, IOfferRepository offerRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.fileOperation = fileOperation ?? throw new ArgumentNullException(nameof(fileOperation));
            this.catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
            this.mediator = mediator;
            this.system = system;
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<ErrorOr<bool>> Handle(RejectCommand command, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            if (command.numberOffer == 0)
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "NumberOffer"));
            }

            //si el estado de la oferta es comprada saca error
            bool IsPurchased = await _offerRepository.OfferIsPurchasedAsync(command.numberOffer);
            if (IsPurchased)
                return Error.Validation(MessageCodes.MessageOfferIsPurchased, GetErrorDescription(MessageCodes.MessageOfferIsPurchased));

            var nameFile = $"COMP_{command.numberOffer}_RECHAZO_{string.Format("{0:ddMMyyyy_HHmm}", ExtensionFormat.DateTimeCO())}.xlsx";
            await this.invoiceDispersionRepository.ChangeStatusToRejectInvoiceAsync(command.numberOffer, userId, CatalogCode_InvoiceDispersionStatus.Rejected);
            var result = await this.invoiceDispersionRepository.ResumeInvoiceExelAsync(command.numberOffer, userId, CatalogCode_InvoiceDispersionStatus.Rejected);
            if (!result.Any())
            {
                return false;
            }
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            var fileExel = this.fileOperation.CreateFileExcelAsync(result, "Transacciones Rechazadas");
            var urlAnexo = await this.storage.UploadAsync(fileExel, pathFileStorage + nameFile);
            var time = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableLink);
            var urlScurityFileExel = await this.storage.GenerateSecureDownloadUrlAsync(urlAnexo.ToString(), nameFile, Convert.ToInt32(time.Descripton));
            var trantations = result is null || !result.Any() ? string.Empty : string.Join(",", result.Select(s => s.Nro_Transaccion));
            await this.mediator.Publish(new EmailToAdminBuyerRejectCommand { NumberOffer = command.numberOffer, TransactionNumber = trantations, NameBuyer = result?.FirstOrDefault()?.Nombre_Comprador, UrlFileExcel = urlScurityFileExel });
            return true;
        }
    }
}