///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class DocumentRepository : Repository<DocumentInfo>, IDocumentRepository
    {
        private yourInvoiceCommonDbContext _db;

        public DocumentRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
    }
}