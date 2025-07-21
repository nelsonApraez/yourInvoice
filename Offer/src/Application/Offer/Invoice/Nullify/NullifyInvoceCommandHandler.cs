///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.Nullify
{
    public sealed class NullifyInvoceCommandHandler : IRequestHandler<NullifyInvoceCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository offerRepository;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IEventNotificationsRepository eventNotificationsRepository;
        private readonly IInvoiceEventRepository invoiceEventRepository;
        private readonly IMoneyTransferRepository moneyTransferRepository;
        private readonly IStorage storage;
        private readonly IUnitOfWork _unitOfWork;

        public NullifyInvoceCommandHandler(IOfferRepository offerRepository, IInvoiceRepository invoiceRepository,
            IDocumentRepository documentRepository, IEventNotificationsRepository eventNotificationsRepository,
            IInvoiceEventRepository invoiceEventRepository, IUnitOfWork unitOfWork
            , IMoneyTransferRepository moneyTransferRepository, IStorage storage)
        {
            this.offerRepository = offerRepository;
            this.invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            this.documentRepository = documentRepository;
            this.eventNotificationsRepository = eventNotificationsRepository;
            this.invoiceEventRepository = invoiceEventRepository;
            this.moneyTransferRepository = moneyTransferRepository ?? throw new ArgumentNullException(nameof(moneyTransferRepository));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(NullifyInvoceCommand command, CancellationToken cancellationToken)
        {
            if (command is null || command.offerId == Guid.Empty)
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "OfferId"));
            }

            //si el estado de la oferta no es en progreso saca error
            if (!await this.offerRepository.OfferIsInProgressAsync(command.offerId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            await NullifyDocumentsStorageAsync(command);
            await NullyfyBeneficiaryAsync(command);
            await this.invoiceEventRepository.NullyfyAsync(command.offerId);
            await this.eventNotificationsRepository.NullyfyAsync(command.offerId);
            await NullifyInvoicesAsync(command);
            await this.documentRepository.NullyfyDocumentsAsync(command.offerId);
            await this.offerRepository.DeleteAsync(command.offerId);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result >= 0;
        }

        private async Task NullifyInvoicesAsync(NullifyInvoceCommand command)
        {
            var invoices = await this.invoiceRepository.FindByOfferId(command.offerId);

            foreach (var invoice in invoices)
            {
                var documents = await documentRepository.GetDocumentsByOfferAndRelatedAsync((Guid)invoice.OfferId, invoice.Id);
                foreach (var itemDoc in documents)
                {
                    await storage.DeleteBlobByUrlAsync(itemDoc.Url + itemDoc.Name);
                }
            }
            await this.invoiceRepository.NullyfyAsync(command.offerId);
        }

        private async Task NullifyDocumentsStorageAsync(NullifyInvoceCommand command)
        {
            var documents = await documentRepository.GetAllDocumentsByOfferAsync(command.offerId);
            foreach (var itemDoc in documents)
            {
                await storage.DeleteBlobByUrlAsync(itemDoc.Url);
            }
        }

        private async Task NullyfyBeneficiaryAsync(NullifyInvoceCommand command)
        {
            var beneficiaries = await this.moneyTransferRepository.GetAllByOfferId(command.offerId);
            foreach (var beneficiary in beneficiaries)
            {
                await this.moneyTransferRepository.DeleteAsync(beneficiary.Id);
                var documents = await documentRepository.GetDocumentsByOfferAndRelatedAsync(beneficiary.OfferId, beneficiary.Id);
                foreach (var itemDoc in documents)
                {
                    await storage.DeleteBlobByUrlAsync(itemDoc.Url);
                }
            }
        }
    }
}