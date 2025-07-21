///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Application.Offer.ListSellerDocs
{
    public record ListSellerDocsQuery(Guid offerId) : IRequest<ErrorOr<List<ListDocsResponse>>>;
}