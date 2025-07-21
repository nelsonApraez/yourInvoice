///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Caching.Memory;
using yourInvoice.Common.Entities;
using System.Linq.Expressions;

namespace yourInvoice.Common.Extension
{
    public static class LinqExtension
    {
        public static IMemoryCache _cache;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.AndAlso(left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // need to detect whether they use the same
            // parameter instance; if not, they need fixing
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                // simple version
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param);
            }
            // otherwise, keep expr1 "as is" and invoke expr2
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    expr1.Body,
                    Expression.Invoke(expr2, param)), param);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.OrElse(left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationInfo paginator)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            string[] properties = paginator.ColumnOrder.Split('.');
            MemberExpression mex = Expression.Property(parameter, properties[0]);
            for (int i = 1; i < properties.Length; i++)
            {
                mex = Expression.Property(mex, properties[i]);
            }
            var exp = Expression.Lambda(mex, parameter);

            string method = paginator.SortDirection == SortDirection.Asc ? "OrderBy" : "OrderByDescending";

            Type[] types = new Type[] { query.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            var temp = query.Provider.CreateQuery<T>(mce)
                .Skip(paginator.StartIndex);

            return paginator.PageSize == 0 ?
                temp :
                temp.Take(paginator.PageSize);
        }

        public static List<T> ToListCache<T>(this IQueryable<T> @this)
        {
            if (_cache == null)
                throw new ArgumentException("Debes definir el cache antes de usar este metodo");

            List<T> items = null;
            var key = typeof(T).FullName;
            var cacheItems = (List<T>)_cache.Get(key);
            if (cacheItems != null && cacheItems.Count() > 0)
            {
                items = cacheItems;
            }
            else
            {
                items = @this.ToList();
                _cache.Set<List<T>>(key, items, DateTimeOffset.Now.AddHours(12));
            }
            return items;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"No property '{propertyName}' on type '{typeof(T)}'");

            return source.OrderBy(x => propertyInfo.GetValue(x, null));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"No property '{propertyName}' on type '{typeof(T)}'");

            return source.OrderByDescending(x => propertyInfo.GetValue(x, null));
        }
    }
}