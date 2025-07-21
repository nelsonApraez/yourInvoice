///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Common.Persistence.Repositories;

namespace yourInvoice.Common.Business.CatalogModule
{
    public class CatalogBusiness : ICatalogBusiness
    {
        private readonly ICatalogRepository repository;

        public CatalogBusiness(ICatalogRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CatalogItemInfo> GetByIdAsync(Guid id)
        {
            var result = await repository.GetByIdAsync(id);

            return result ?? new();
        }

        public async Task<IEnumerable<CatalogItemInfo>> GetByParentAsync(Guid parentId)
        {
            var result = await repository.GetByParentAsync(parentId);

            return result ?? Enumerable.Empty<CatalogItemInfo>();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await repository.IsExistAsync(id);

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogAsync(string catalogName)
        {
            var result = await repository.ListByCatalogAsync(catalogName);

            return result ?? Enumerable.Empty<CatalogItemInfo>();
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogParenIdAsync(Guid parentId)
        {
            var result = await repository.ListByCatalogParenIdAsync(parentId);

            return result ?? Enumerable.Empty<CatalogItemInfo>();
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogOrderDescriptionAsync(string catalogName)
        {
            var result = await repository.ListByCatalogOrderDescriptionAsync(catalogName);

            return result ?? Enumerable.Empty<CatalogItemInfo>();
        }
    }
}