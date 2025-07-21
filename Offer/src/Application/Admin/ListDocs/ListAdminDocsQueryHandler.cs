///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Offer.ListDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.ListDocs
{
    public sealed class ListAdminDocsQueryHandler : IRequestHandler<ListAdminDocsQuery, ErrorOr<Dictionary<string, List<ListDocsResponse>>>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IOfferRepository _offerRepository;
        private readonly IInvoiceDispersionRepository _invoiceDispersionRepository;

        public ListAdminDocsQueryHandler(IDocumentRepository repository, IOfferRepository offerRepository, IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<Dictionary<string, List<ListDocsResponse>>>> Handle(ListAdminDocsQuery query, CancellationToken cancellationToken)
        {
            InvoiceDispersion invoice = await _invoiceDispersionRepository.GetByTransactionNumberAsync(query.transactionId);

            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(invoice.OfferNumber);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var document = await _repository.GetAllDocumentsByOfferAsync(offer.Id);

            //var apendix = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Appendix);
            var commercialOffer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);

            if (commercialOffer == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            var moneyTransferInstruction = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var endorsement = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var endorsementNotification = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);

            var moneyTransferInstructionBuyer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == invoice.BuyerId);
            var commercialOfferBuyer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == invoice.BuyerId);
            var purchaseCertificate = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == invoice.BuyerId);
            var transferSupportBuyer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer && x.CreatedBy == invoice.BuyerId);

            List<ListDocsResponse> docs = new()
            {
                 //new ListDocsResponse { Name = apendix.Name,
                 //    DocumentId = apendix.Id, IsSigned = (bool)apendix.IsSigned, Size = apendix.FileSize},
                 new ListDocsResponse { Name = commercialOffer.Name,
                     DocumentId = commercialOffer.Id , IsSigned = (bool)commercialOffer.IsSigned, Size = commercialOffer.FileSize},
                 new ListDocsResponse { Name = endorsement.Name,
                     DocumentId = endorsement.Id , IsSigned = (bool)endorsement.IsSigned, Size = endorsement.FileSize},
                 new ListDocsResponse { Name = endorsementNotification.Name,
                     DocumentId = endorsementNotification.Id , IsSigned = (bool)endorsementNotification.IsSigned, Size = endorsementNotification.FileSize},
                 new ListDocsResponse { Name = moneyTransferInstruction.Name,  IsSigned = (bool)moneyTransferInstruction.IsSigned,
                      DocumentId = moneyTransferInstruction.Id , Size = moneyTransferInstruction.FileSize},
            };

            List<ListDocsResponse> docsBuyer = new();
            if (commercialOfferBuyer != null)
            {
                docsBuyer.Add(new ListDocsResponse
                {
                    Name = commercialOfferBuyer.Name,
                    DocumentId = commercialOfferBuyer.Id,
                    IsSigned = (bool)commercialOfferBuyer.IsSigned,
                    Size = commercialOfferBuyer.FileSize
                });
            }

            if (purchaseCertificate != null)
            {
                docsBuyer.Add(new ListDocsResponse
                {
                    Name = purchaseCertificate.Name,
                    DocumentId = purchaseCertificate.Id,
                    IsSigned = (bool)purchaseCertificate.IsSigned,
                    Size = purchaseCertificate.FileSize
                });
            }

            if (moneyTransferInstructionBuyer != null)
            {
                docsBuyer.Add(new ListDocsResponse
                {
                    Name = moneyTransferInstructionBuyer.Name,
                    IsSigned = (bool)moneyTransferInstructionBuyer.IsSigned,
                    DocumentId = moneyTransferInstructionBuyer.Id,
                    Size = moneyTransferInstructionBuyer.FileSize
                });
            }

            if (transferSupportBuyer != null)
            {
                docsBuyer.Add(new ListDocsResponse
                {
                    Name = transferSupportBuyer.Name,
                    IsSigned = (bool)transferSupportBuyer.IsSigned,
                    DocumentId = transferSupportBuyer.Id,
                    Size = transferSupportBuyer.FileSize
                });
            }

            Dictionary<string, List<ListDocsResponse>> Documents = new() { { "documentsOffer", docs }, { "documentsBuyer", docsBuyer } };

            return Documents;
        }
    }
}