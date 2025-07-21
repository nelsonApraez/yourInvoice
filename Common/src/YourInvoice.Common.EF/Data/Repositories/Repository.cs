///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;
using System.Linq.Expressions;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : ModelBase
    {
        protected readonly DbSet<TModel> ModelDbSets;
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException();
            ModelDbSets = _dbContext.Set<TModel>();
        }

        public TModel Add(TModel entity)
        {
            entity.Id = (entity.Id == Guid.Empty) ? Guid.NewGuid() : entity.Id;
            return ModelDbSets.Add(entity).Entity;
        }

        public async Task<TModel> AddAsync(TModel entity)
        {
            entity.Id = (entity.Id == Guid.Empty) ? Guid.NewGuid() : entity.Id;
            await Task.Run(() => _dbContext.Entry(entity).State = EntityState.Added);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TModel> entities)
        {
            foreach (var entity in entities)
            {
                entity.Id = (entity.Id == Guid.Empty) ? Guid.NewGuid() : entity.Id;
            }

            await ModelDbSets.AddRangeAsync(entities);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<TModel> GetAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await ModelDbSets.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await ModelDbSets.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<bool> Any(Expression<Func<TModel, bool>> predicate)
        {
            return await ModelDbSets.AnyAsync(predicate);
        }

        public IQueryable<TModel> Queryable(Expression<Func<TModel, bool>> predicate)
        {
            return ModelDbSets.Where(predicate);
        }

        public void Remove(TModel entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached) ModelDbSets.Attach(entity);

            ModelDbSets.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TModel> entities)
        {
            foreach (var entity in entities)
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached) ModelDbSets.Attach(entity);

                ModelDbSets.Remove(entity);
            }
        }

        public void Update(TModel entity)
        {
            ModelDbSets.Attach(entity);
        }

        public async Task UpdateAsync(TModel entity)
        {
            await Task.Run(() => _dbContext.Entry(entity).State = EntityState.Modified);
        }
    }
}