///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Common;
using System.Linq.Expressions;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        private readonly ISystem system;

        protected ApplicationDbContext context { get; set; }

        protected RepositoryBase(ApplicationDbContext context, ISystem system)
        {
            this.context = context;
            this.system = system;
        }

        public IQueryable<T> FindAll() => this.context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            context.Set<T>().Where(expression).AsNoTracking();

        public T Create(T entity)
        {
            entity = SetValuePropertyCreate(entity);
            return this.context.Set<T>().Add(entity).Entity;
        }

        public T Update(T entity)
        {
            entity = SetValuePropertyUpdate(entity);
            return this.context.Set<T>().Update(entity).Entity;
        }

        public void Delete(T entity) => this.context.Set<T>().Remove(entity);

        private T SetValuePropertyCreate(T property)
        {
            property = GetPropertyTypeGuidValue(property, "Id");
            property = GetPropertyTypeGuidValue(property, "CreatedBy");
            property = GetPropertyTypeGuidValue(property, "ModifiedBy");
            return property;
        }

        private T SetValuePropertyUpdate(T property)
        {
            property = GetPropertyTypeGuidValue(property, "Id");
            property = GetPropertyTypeGuidValue(property, "ModifiedBy");
            return property;
        }

        private Guid GetUser()
        {
            return this.system.User.Id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
            }
        }

        private T GetPropertyTypeGuidValue(T property, string nameProperty)
        {
            var valueCreatedBy = property?.GetType()?.GetProperty(nameProperty)?.GetValue(property);
            Guid createBy = valueCreatedBy is null ? GetUser() : (Guid)(valueCreatedBy);
            property?.GetType()?.GetProperty(nameProperty)?.SetValue(property, createBy);

            return property;
        }
    }
}