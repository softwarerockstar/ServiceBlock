using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBlock.Foundation
{
    [ExcludeFromCodeCoverage]
    public static class IQueryableExtensions
    {
        public static Expression<Func<T, object>> DynamicSortExpression<T>(this IQueryable target, string sortColumnName)
        {
            // Using a custom LINQ expression to sort by sortColumnName
            var param = Expression.Parameter(typeof(T), "x");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Property(param, sortColumnName), param);

            return sortExpression;
        }

        private const string Ascending = "ASC";

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, string direction)
        {
            return direction == Ascending ? source.OrderBy(property) : source.OrderByDescending(property);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property, string direction)
        {
            return direction == Ascending ? source.ThenBy(property) : source.ThenByDescending(property);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        public static IQueryable<T> IncludePropertyListCsv<T>(this IQueryable<T> query, string includeProperties) where T : class
        {   
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query;
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var pi in props.Select(prop => type.GetProperty(prop)))
            {
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;

        }
    }
}
