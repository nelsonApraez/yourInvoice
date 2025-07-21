///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetByName
{
    public record GetCatalogByNameQuery(string catalogName) : IRequest<ErrorOr<IEnumerable<CatalogItemInfo>>>;
}