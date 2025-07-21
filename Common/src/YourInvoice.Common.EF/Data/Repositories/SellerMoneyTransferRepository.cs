///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class SellerMoneyTransferRepository : Repository<SellerMoneyTransferInfo>, ISellerMoneyTransferRepository
    {
        private yourInvoiceCommonDbContext _db;

        public SellerMoneyTransferRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
    }
}