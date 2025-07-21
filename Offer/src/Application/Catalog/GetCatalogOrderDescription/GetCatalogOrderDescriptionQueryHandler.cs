///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetCatalogOrderDescription
{
    public class GetCatalogOrderDescriptionQueryHandler : IRequestHandler<GetCatalogOrderDescriptionQuery, ErrorOr<IEnumerable<CatalogItemInfo>>>
    {
        private readonly ICatalogBusiness _catalogBusiness;

        public GetCatalogOrderDescriptionQueryHandler(ICatalogBusiness catalogBusiness)
        {
            this._catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<IEnumerable<CatalogItemInfo>>> Handle(GetCatalogOrderDescriptionQuery query, CancellationToken cancellationToken)
        {
            var catalogItemInfo = await _catalogBusiness.ListByCatalogOrderDescriptionAsync(query.CatalogName);

            return catalogItemInfo?.ToList();
        }
    }
}