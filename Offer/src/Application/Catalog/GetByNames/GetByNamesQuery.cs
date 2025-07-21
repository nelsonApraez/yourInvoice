///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetByNames;

public record GetByNamesQuery(string[] Names) : IRequest<ErrorOr<Dictionary<string, IEnumerable<CatalogItemInfo>>>>;