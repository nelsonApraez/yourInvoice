///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class UnitOfWorkLink : IUnitOfWorkLink
    {
        public readonly LinkDbContext _dbContext;

        public UnitOfWorkLink(LinkDbContext dbContext)
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