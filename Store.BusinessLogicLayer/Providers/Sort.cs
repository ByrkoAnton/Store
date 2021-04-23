using Microsoft.AspNetCore.Http;
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
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_PROP_NAME, StatusCodes.Status400BadRequest);
            }

            var parameter = Expression.Parameter(typeof(T), Constants.Constants.Variables.DEFAULT);
            var selectorExpr = Expression.Lambda(Expression.Property(parameter, prop), parameter);

            Expression queryExpr = source.Expression;

            queryExpr = Expression.Call
                (typeof(Queryable), asc ? Constants.Constants.LinqOperators.ORDER_BY : Constants.Constants.LinqOperators.ORDER_BY_DSC, new Type[] 
                { source.ElementType, searchProperty.PropertyType }, queryExpr, selectorExpr);

            return source.Provider.CreateQuery<T>(queryExpr);
        }
    }
}
