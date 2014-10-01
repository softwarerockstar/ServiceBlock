using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace VHA.ServiceFoundation
{
    [ExcludeFromCodeCoverage]
    public static class PagingUtility
    {
        public static PagingList<T> ExecuteQueryWithCriteria<T>(
            IQueryable<T> query,
            Criteria criteria) where T : class
        {
            var toReturn = new PagingList<T>();

            IQueryable<T> queryBuilder = query;

            if (criteria.FilterFieldName != null && criteria.FilterFieldValue != null)
            {
                queryBuilder = query.Where(
                    DynamicExpressionBuilder.BuildFilterExpression<T>(
                        criteria.FilterFieldName,
                        criteria.FilterFieldValue,
                        StringFilterOperator.Contains));
            }

            toReturn.TotalCount = queryBuilder.Count();

            if (criteria.SortFieldName != null)
                queryBuilder = queryBuilder.OrderBy(
                    property: criteria.SortFieldName,
                    direction: (criteria.SortDirection == SortDirection.Ascending) ? "ASC" : "DESC");
            
            if (criteria.PageSize != -1)
                queryBuilder = queryBuilder
                    .Skip(criteria.PageSize * (criteria.PageNumber - 1))
                    .Take(criteria.PageSize);

            toReturn.AddRange(queryBuilder);
            return toReturn;
        }
    }
}
