using System;
using System.Linq;
using System.Linq.Expressions;

namespace Store.BusinessLogicLayer.Providers
{

    public static class Sort
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string prop, bool asc) where T: class
        {
            var searchProperty = typeof(T).GetProperty(prop);

            if (searchProperty is null)
            {
                throw new CustomExeption("property", 400);
            }

            var parameter = Expression.Parameter(typeof(T), "o");
            var selectorExpr = Expression.Lambda(Expression.Property(parameter, prop), parameter);

            Expression queryExpr = source.Expression;

            queryExpr = Expression.Call
                (typeof(Queryable), asc ? "OrderBy" : "OrderByDescending", new Type[] 
                { source.ElementType, searchProperty.PropertyType }, queryExpr, selectorExpr);

            return source.Provider.CreateQuery<T>(queryExpr);
        }
    }
}
