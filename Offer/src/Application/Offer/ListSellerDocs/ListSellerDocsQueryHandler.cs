///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Application.Offer.ListSellerDocs
{
    public sealed class ListSellerDocsQueryHandler : IRequestHandler<ListSellerDocsQuery, ErrorOr<List<ListDocsResponse>>>
    {
        private readonly IDocumentRepository _repository;
        private readonly ISystem system;
        private readonly IHttpContextAccessor httpContext;

        public ListSellerDocsQueryHandler(IDocumentRepository repository, ISystem system, IHttpContextAccessor httpContext)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system;
            this.httpContext = httpContext;
        }

        public async Task<ErrorOr<List<ListDocsResponse>>> Handle(ListSellerDocsQuery query, CancellationToken cancellationToken)
        {
            var document = await _repository.GetAllDocumentsByOfferAsync(query.offerId);

            var commercialOffer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);

            if (commercialOffer == null)
                return new List<ListDocsResponse>();

            var moneyTransferInstruction = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var endorsement = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var endorsementNotification = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);

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