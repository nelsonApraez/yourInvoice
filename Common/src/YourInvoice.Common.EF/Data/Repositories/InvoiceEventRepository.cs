///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class InvoiceEventRepository : Repository<InvoiceEventInfo>, IInvoiceEventRepository
    {
        private yourInvoiceCommonDbContext _db;

        public InvoiceEventRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<bool> AddInvoiceEventAsync(List<InvoiceEventInfo> invoiceEvents)
        {
            await base.AddRangeAsync(invoiceEvents);

            return true;
        }
    }
}