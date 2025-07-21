///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.ListDocs
{
    public record ListDocsQuery(Guid offerId) : IRequest<ErrorOr<List<ListDocsResponse>>>;
}