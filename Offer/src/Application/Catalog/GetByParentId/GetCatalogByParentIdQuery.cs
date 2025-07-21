///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetByParentId;

public record GetCatalogByParentIdQuery(Guid ParentId) : IRequest<ErrorOr<IEnumerable<CatalogItemInfo>>>;