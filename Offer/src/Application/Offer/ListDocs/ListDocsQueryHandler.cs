///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.ListDocs
{
    public sealed class ListDocsQueryHandler : IRequestHandler<ListDocsQuery, ErrorOr<List<ListDocsResponse>>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IOfferRepository _offerRepository;

        public ListDocsQueryHandler(IDocumentRepository repository, IOfferRepository offerRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<ErrorOr<List<ListDocsResponse>>> Handle(ListDocsQuery query, CancellationToken cancellationToken)
        {
            Domain.Offer offer = await _offerRepository.GetByIdAsync(query.offerId);
            if (offer == null || offer.StatusId != CatalogCode_OfferStatus.InProgress)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documents = await _repository.GetAllDocumentsByOfferAsync(offer.Id);

            var commercialOffer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);

            if (commercialOffer == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            var moneyTransferInstruction = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var endorsement = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var endorsementNotification = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);

            List<ListDocsResponse> docs = new()
            {
                 new ListDocsResponse { Name = commercialOffer.Name,
                     DocumentId = commercialOffer.Id , IsSigned = (bool)commercialOffer.IsSigned, Size = commercialOffer.FileSize},
                 new ListDocsResponse { Name = endorsement.Name,
                     DocumentId = endorsement.Id , IsSigned = (bool)endorsement.IsSigned, Size = endorsement.FileSize},
                 new ListDocsResponse { Name = endorsementNotification.Name,
                     DocumentId = endorsementNotification.Id , IsSigned = (bool)endorsementNotification.IsSigned, Size = endorsementNotification.FileSize},
                 new ListDocsResponse { Name = moneyTransferInstruction.Name,  IsSigned = (bool)moneyTransferInstruction.IsSigned,
                      DocumentId = moneyTransferInstruction.Id , Size = moneyTransferInstruction.FileSize},
            };

            return docs;
        }
    }
}