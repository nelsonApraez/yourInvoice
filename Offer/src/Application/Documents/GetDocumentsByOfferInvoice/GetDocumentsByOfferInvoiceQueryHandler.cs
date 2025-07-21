///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Documents.Common;
using yourInvoice.Offer.Domain.Documents;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Documents.GetDocumentsByOfferInvoice
{
    public sealed class GetDocumentsByOfferInvoiceQueryHandler : IRequestHandler<GetDocumentsByOfferInvoiceQuery, ErrorOr<IReadOnlyList<DocumentResponse>>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorage storage;

        public GetDocumentsByOfferInvoiceQueryHandler(IDocumentRepository documentRepository, IStorage storage)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            this.storage = storage;
        }

        public async Task<ErrorOr<IReadOnlyList<DocumentResponse>>> Handle(GetDocumentsByOfferInvoiceQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(query.request.FormatTypeFile))
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "FormatTypeFile"));
            }

            var formatTypeFile = !query.request.FormatTypeFile.Contains(".") ? $".{query.request.FormatTypeFile}" : query.request.FormatTypeFile;
            IReadOnlyList<Document> document = await _documentRepository.GetDocumentsByOfferInvoiceAsync(query.request.OfferId, query.request.InvoiceId, formatTypeFile.ToLowerInvariant());
            var response = document.Select(s =>
            new DocumentResponse
            (
                s.Id,
                s.OfferId,
                s.RelatedId,
                s.Name,
                s.TypeId,
                s.IsSigned,
                this.storage.GenerateSecureDownloadUrlAsync(s.Url + s.Name, s.Name).Result,
                query.request.FormatTypeFile.ToUpperInvariant()
            )).ToList();

            return response;
        }
    }
}