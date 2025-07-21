///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.EF.Data.IRepositories;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class UnitOfWorkCommonEF : IUnitOfWorkCommonEF
    {
        public readonly yourInvoiceCommonDbContext _dbContext;

        public UnitOfWorkCommonEF(yourInvoiceCommonDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                if (result == 0)
                    throw new ArgumentException("No se pudo realizar el commit sobre la DB");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}