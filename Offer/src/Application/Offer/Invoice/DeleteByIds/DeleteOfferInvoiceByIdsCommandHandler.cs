///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.DeleteByIds
{
    public sealed class DeleteOfferInvoiceByIdsCommandHandler : IRequestHandler<DeleteOfferInvoiceByIdsCommand, ErrorOr<bool>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IStorage storage;
        private readonly IInvoiceEventRepository invoiceEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOfferRepository _offerRepository;

        public DeleteOfferInvoiceByIdsCommandHandler(IInvoiceRepository invoiceRepository, IDocumentRepository documentRepository,
            IUnitOfWork unitOfWork, IStorage storage, IInvoiceEventRepository invoiceEventRepository, IOfferRepository offerRepository)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.invoiceEventRepository = invoiceEventRepository;
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            this.documentRepository = documentRepository;
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));            
        }

        public async Task<ErrorOr<bool>> Handle(DeleteOfferInvoiceByIdsCommand command, CancellationToken cancellationToken)
        {
            if (command is null || !command.invoiceIds.Any())
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "invoiceIds"));
            }

            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressByInvoiceIdAsync(command.invoiceIds.FirstOrDefault()))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            foreach (var invoiceId in command.invoiceIds)
            {
                var invoice = await _invoiceRepository.GetById(invoiceId);
                var documents = await documentRepository.GetDocumentsByOfferAndRelatedAsync((Guid)invoice.FirstOrDefault().OfferId, invoice.FirstOrDefault().Id);
                foreach (var itemDoc in documents)
                {
                    await storage.DeleteBlobByUrlAsync(itemDoc.Url + itemDoc.Name);
                }
            }

            await this.documentRepository.DeleteDocumentsAsync(command.invoiceIds);
            await this.invoiceEventRepository.DeleteAsync(command.invoiceIds);
            await _invoiceRepository.DeleteAsync(command.invoiceIds);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result >= 0;
        }
    }
}