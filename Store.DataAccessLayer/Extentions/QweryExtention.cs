using Microsoft.AspNetCore.Http;
using Store.Sharing.Constants;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Store.DataAccessLayer.Extentions
{
    public static class QweryExtention
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, PropertyInfo prop, bool asc) where T: class
        {
            var parameter = Expression.Parameter(typeof(T), Constants.Variables.DEFAULT);
            var selectorExpr = Expression.Lambda(Expression.Property(parameter, prop), parameter);

            Expression queryExpr = source.Expression;

            queryExpr = Expression.Call
                (typeof(Queryable), asc ? Constants.LinqOperators.ORDER : Constants.LinqOperators.ORDER_DSC, new Type[] 
                { source.ElementType, prop.PropertyType }, queryExpr, selectorExpr);

            return source.Provider.CreateQuery<T>(queryExpr);
        }
    }
}
