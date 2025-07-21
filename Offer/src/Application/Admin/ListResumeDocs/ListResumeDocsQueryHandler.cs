///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Offer.ListDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.ListResumeDocs
{
    public sealed class ListResumeDocsQueryHandler : IRequestHandler<ListResumeDocsQuery, ErrorOr<List<ListDocsResponse>>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IOfferRepository _offerRepository;

        public ListResumeDocsQueryHandler(IDocumentRepository repository, IOfferRepository offerRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<ErrorOr<List<ListDocsResponse>>> Handle(ListResumeDocsQuery query, CancellationToken cancellationToken)
        {
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(query.offerId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documents = await _repository.GetAllDocumentsByOfferAsync(offer.Id);

            var moneyTransferInstructions = documents.Where(x => x.TypeId == CatalogCode_DocumentType.DocumentsUploadByUserOnResume);

            List<ListDocsResponse> docs = new();

            foreach (var document in moneyTransferInstructions)
            {
                docs.Add(new ListDocsResponse
                {
                    Name = document.Name,
                    IsSigned = (bool)document.IsSigned,
                    DocumentId = document.Id,
                    Size = document.FileSize
                });
            }

            return docs;
        }
    }
}