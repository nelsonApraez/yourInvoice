///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Documents.Common;

namespace yourInvoice.Offer.Application.Documents.GetDocumentsByOfferInvoice
{
    public record GetDocumentsByOfferInvoiceQuery(DocumentByOfferInvoiceRequest request) : IRequest<ErrorOr<IReadOnlyList<DocumentResponse>>>;
}