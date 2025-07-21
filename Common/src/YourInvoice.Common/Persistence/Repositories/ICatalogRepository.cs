///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Persistence.Repositories
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<CatalogItemInfo>> ListByCatalogAsync(string catalogName);

        Task<CatalogItemInfo> GetByIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);

        Task<IEnumerable<CatalogItemInfo>> GetByParentAsync(Guid parentId);
        Task<IEnumerable<CatalogItemInfo>> ListByCatalogParenIdAsync(Guid parentId);
        Task<IEnumerable<CatalogItemInfo>> ListByCatalogOrderDescriptionAsync(string catalogName);
    }
}