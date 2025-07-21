///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetByParentId
{
    public class GetCatalogByParentIdQueryHandler : IRequestHandler<GetCatalogByParentIdQuery, ErrorOr<IEnumerable<CatalogItemInfo>>>
    {
        private readonly ICatalogBusiness _catalogBusiness;

        public GetCatalogByParentIdQueryHandler(ICatalogBusiness catalogBusiness)
        {
            this._catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<IEnumerable<CatalogItemInfo>>> Handle(GetCatalogByParentIdQuery query, CancellationToken cancellationToken)
        {
            var catalogItemInfo = await _catalogBusiness.GetByParentAsync(query.ParentId);

            return catalogItemInfo?.ToList();
        }
    }
}