///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Offer.ListDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.ListDocs
{
    public sealed class ListDocsBuyerQueryHandler : IRequestHandler<ListDocsBuyerQuery, ErrorOr<List<ListDocsResponse>>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IOfferRepository _offerRepository;
        private readonly ISystem system;

        public ListDocsBuyerQueryHandler(IDocumentRepository repository, IOfferRepository offerRepository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<List<ListDocsResponse>>> Handle(ListDocsBuyerQuery query, CancellationToken cancellationToken)
        {
            var buyerId = this.system.User.Id;
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(query.numberOffer);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documents = await _repository.GetAllDocumentsByOfferAsync(offer.Id);

            var moneyTransferInstructionBuyer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == buyerId);
            var commercialOfferBuyer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == buyerId);
            var purchaseCertificate = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == buyerId);
            var transferSupportBuyer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer && x.CreatedBy == buyerId);

            if (commercialOfferBuyer == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            List<ListDocsResponse> docs = new()
            {
                new ListDocsResponse { Name = commercialOfferBuyer.Name,
                    DocumentId = commercialOfferBuyer.Id, IsSigned = (bool)commercialOfferBuyer.IsSigned, Size = commercialOfferBuyer.FileSize},
                 new ListDocsResponse { Name = purchaseCertificate.Name,
                     DocumentId = purchaseCertificate.Id , IsSigned = (bool)purchaseCertificate.IsSigned, Size = purchaseCertificate.FileSize},
            };

            Validations(moneyTransferInstructionBuyer, transferSupportBuyer, docs);

            return docs;
        }

        private static void Validations(Document moneyTransferInstructionBuyer, Document transferSupportBuyer, List<ListDocsResponse> docs)
        {
            if (moneyTransferInstructionBuyer != null)
            {
                docs.Add(new ListDocsResponse
                {
                    Name = moneyTransferInstructionBuyer.Name,
                    IsSigned = (bool)moneyTransferInstructionBuyer.IsSigned,
                    DocumentId = moneyTransferInstructionBuyer.Id,
                    Size = moneyTransferInstructionBuyer.FileSize
                });
            }

            if (transferSupportBuyer != null)
            {
                docs.Add(new ListDocsResponse
                {
                    Name = transferSupportBuyer.Name,
                    IsSigned = (bool)transferSupportBuyer.IsSigned,
                    DocumentId = transferSupportBuyer.Id,
                    Size = transferSupportBuyer.FileSize
                });
            }
        }
    }
}