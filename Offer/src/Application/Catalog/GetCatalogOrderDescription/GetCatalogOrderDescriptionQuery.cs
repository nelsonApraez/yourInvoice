///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetCatalogOrderDescription;

public record GetCatalogOrderDescriptionQuery(string CatalogName) : IRequest<ErrorOr<IEnumerable<CatalogItemInfo>>>;