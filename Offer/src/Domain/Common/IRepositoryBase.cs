///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Linq.Expressions;

namespace yourInvoice.Offer.Domain.Common
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        T Create(T entity);

        T Update(T entity);

        void Delete(T entity);
    }
}