///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Catalog.GetByNames
{
    public class GetByNamesQueryHandler : IRequestHandler<GetByNamesQuery, ErrorOr<Dictionary<string, IEnumerable<CatalogItemInfo>>>>
    {
        private readonly ICatalogBusiness _catalogBusiness;

        public GetByNamesQueryHandler(ICatalogBusiness catalogBusiness)
        {
            this._catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<Dictionary<string, IEnumerable<CatalogItemInfo>>>> Handle(GetByNamesQuery query, CancellationToken cancellationToken)
        {
            var dictionary = new Dictionary<string, IEnumerable<CatalogItemInfo>>();
            foreach (var catalog in query.Names)
            {
                var catalogItemInfo = await _catalogBusiness.ListByCatalogAsync(catalog);
                dictionary[catalog] = catalogItemInfo;
            }
            return dictionary;
        }
    }
}