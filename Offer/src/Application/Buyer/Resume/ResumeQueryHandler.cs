///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.Resume
{
    public sealed class ResumeQueryHandler : IRequestHandler<ResumeQuery, ErrorOr<ResumeResponse>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly Domain.Documents.IDocumentRepository _repository;
        private readonly Domain.Offers.IOfferRepository _offerRepository;
        private readonly ISystem system;

        public ResumeQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository, Domain.Documents.IDocumentRepository repository, Domain.Offers.IOfferRepository offerRepository, ISystem system)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            this.system = system;
        }

        public async Task<ErrorOr<ResumeResponse>> Handle(ResumeQuery query, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            var result = await this.invoiceDispersionRepository.GetResumeAsync(query.numberOffer, userId);

            if (result is null)
            {
                return Error.Validation(MessageCodes.NoExistResumen, GetErrorDescription(MessageCodes.NoExistResumen));
            }

            return await AddDocuments(query, result, userId);
        }

        private async Task<ErrorOr<ResumeResponse>> AddDocuments(ResumeQuery query, ResumeResponse result, Guid userId)
        {
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(query.numberOffer);

            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documentos = await _repository.GetAllDocumentsByOfferAsync(offer.Id);
            var commercialOffer = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);

            if (commercialOffer == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            var moneyTransferInstruction = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var endorsement = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var endorsementNotification = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);

            var moneyTransferInstructionBuyer = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == userId);
            var commercialOfferBuyer = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == userId);
            var purchaseCertificate = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == userId);
            var transferSupportBuyer = documentos.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer && x.CreatedBy == userId);

            List<ListDocsResponse> docus = new()
            {
                 new ListDocsResponse() { Name = commercialOffer.Name,
                     DocumentId = commercialOffer.Id , IsSigned = (bool)commercialOffer.IsSigned, Size = commercialOffer.FileSize},
                 new ListDocsResponse() { Name = endorsement.Name,
                     DocumentId = endorsement.Id , IsSigned = (bool)endorsement.IsSigned, Size = endorsement.FileSize},
                 new ListDocsResponse() { Name = endorsementNotification.Name,
                     DocumentId = endorsementNotification.Id , IsSigned = (bool)endorsementNotification.IsSigned, Size = endorsementNotification.FileSize},
                 new ListDocsResponse() { Name = moneyTransferInstruction.Name,  IsSigned = (bool)moneyTransferInstruction.IsSigned,
                      DocumentId = moneyTransferInstruction.Id , Size = moneyTransferInstruction.FileSize},
            };

            List<ListDocsResponse> docusBuyer = new();

            if (commercialOfferBuyer != null)
            {
                docusBuyer.Add(new ListDocsResponse
                {
                    Name = commercialOfferBuyer.Name,
                    DocumentId = commercialOfferBuyer.Id,
                    IsSigned = (bool)commercialOfferBuyer.IsSigned,
                    Size = commercialOfferBuyer.FileSize
                });
            }
            if (purchaseCertificate != null)
            {
                docusBuyer.Add(new ListDocsResponse
                {
                    Name = purchaseCertificate.Name,
                    DocumentId = purchaseCertificate.Id,
                    IsSigned = (bool)purchaseCertificate.IsSigned,
                    Size = purchaseCertificate.FileSize
                });
            }
            if (moneyTransferInstructionBuyer != null)
            {
                docusBuyer.Add(new ListDocsResponse
                {
                    Name = moneyTransferInstructionBuyer.Name,
                    IsSigned = (bool)moneyTransferInstructionBuyer.IsSigned,
                    DocumentId = moneyTransferInstructionBuyer.Id,
                    Size = moneyTransferInstructionBuyer.FileSize
                });
            }
            if (transferSupportBuyer != null)
            {
                docusBuyer.Add(new ListDocsResponse
                {
                    Name = transferSupportBuyer.Name,
                    IsSigned = (bool)transferSupportBuyer.IsSigned,
                    DocumentId = transferSupportBuyer.Id,
                    Size = transferSupportBuyer.FileSize
                });
            }
            Dictionary<string, List<ListDocsResponse>> Documents = new() { { "documentsOffer", docus }, { "documentsBuyer", docusBuyer } };

            result.Documents = Documents;

            return result;
        }
    }
}