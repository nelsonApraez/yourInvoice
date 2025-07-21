///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Persistence.Repositories
{
    internal class CatalogRepository : ICatalogRepository
    {
        private readonly yourInvoiceCommonDbContext context;

        public CatalogRepository(yourInvoiceCommonDbContext context)
        {
            this.context = context;
        }

        public async Task<CatalogItemInfo> GetByIdAsync(Guid id)
        {
            var result = await this.context.CatalogItems.FirstOrDefaultAsync(c => c.Id == id);

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> GetByParentAsync(Guid parentId)
        {
            var result = await this.context.CatalogItems.Where(c => c.ParentId == parentId && c.Status).OrderBy(x => x.Descripton).ToListAsync();

            return result;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await this.context.CatalogItems.AnyAsync(c => c.Id == id);

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogAsync(string catalogName)
        {
            var result = await this.context.CatalogItems.Where(c => c.CatalogName == catalogName.Trim() && c.Status).OrderBy(x => x.Name).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogOrderDescriptionAsync(string catalogName)
        {
            var result = await this.context.CatalogItems.Where(c => c.CatalogName == catalogName.Trim() && c.Status).OrderBy(x => x.Descripton).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> ListByCatalogParenIdAsync(Guid parentId)
        {
            var result = await this.context.CatalogItems.Where(c => c.ParentId == parentId).OrderBy(o => o.Descripton).ToListAsync();

            return result;
        }
    }
}