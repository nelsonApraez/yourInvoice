///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Catalog.GetByName
{
    public sealed class GetCatalogByNameQueryHandler : IRequestHandler<GetCatalogByNameQuery, ErrorOr<IEnumerable<CatalogItemInfo>>>
    {
        private readonly ICatalogBusiness _catalogBusiness;

        public GetCatalogByNameQueryHandler(ICatalogBusiness catalogBusiness)
        {
            _catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<IEnumerable<CatalogItemInfo>>> Handle(GetCatalogByNameQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<CatalogItemInfo> catalogItemInfo = await _catalogBusiness.ListByCatalogAsync(query.catalogName);
            if (!catalogItemInfo.Any())
            {
                return Error.NotFound(MessageCodes.CatalogNotExist, GetErrorDescription(MessageCodes.CatalogNotExist));
            }

            return catalogItemInfo.ToList();
        }
    }
}