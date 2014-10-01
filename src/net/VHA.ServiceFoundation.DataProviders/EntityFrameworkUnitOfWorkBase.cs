using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation;
using VHA.ServiceFoundation.Data;

namespace VHA.ServiceFoundation.DataProviders
{
    public abstract class EntityFrameworkUnitOfWorkBase : DbContext, IUnitOfWorkEx
    {
        public EntityFrameworkUnitOfWorkBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public void Delete<T>(Expression<Func<T, bool>> selector) where T : class
        {
            var entity = base.Set<T>().SingleOrDefault(selector);

            if (entity != null)
                base.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll<T>(string includeProperties) where T : class
        {
            var toReturn = base.Set<T>()
                               .IncludePropertyListCsv(includeProperties)
                               .ToList();

            return toReturn;
        }

        public long GetCount<T>(Expression<Func<T, bool>> selector) where T : class
        {
            return base.Set<T>().Where(selector).Count();
        }

        public IQueryable<T> GetEntitySet<T>() where T : class
        {
            return base.Set<T>();
        }

        public IEnumerable<T> GetWithSelector<T>(
            Expression<Func<T, bool>> selector,
            string includeProperties = null) where T : class
        {
            return base.Set<T>()
                       .Where(selector)
                       .IncludePropertyListCsv(includeProperties)
                       .ToList();
        }

        public PagingList<T> GetWithCriteria<T>(Criteria criteria, string includeProperties = null) where T : class
        {
            var toReturn = new PagingList<T>();
            var query = base.Set<T>().IncludePropertyListCsv(includeProperties); 
            var queryBuilder = query as IQueryable<T>;            

            if (criteria.FilterFieldName != null && criteria.FilterFieldValue != null)
            {
                queryBuilder = queryBuilder.Where(
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

        public PagingList<T> GetWithCriteria<T>(
            IList<Expression<Func<T, bool>>> selectors,
            string sortFieldName = null,
            SortDirection sortDirection = SortDirection.Ascending,
            int pageSize = 10,
            int pageNumber = 1,
            string includeProperties = null) where T : class
        {
            var toReturn = new PagingList<T>();
            var query = base.Set<T>()
                .IncludePropertyListCsv(includeProperties)
                .AsQueryable();

            foreach (var selector in selectors)
                query = query.Where(selector);

            var records = query.ToList();

            toReturn.TotalCount = records.Count;

            if (sortFieldName != null)
                records = records.AsQueryable<T>().OrderBy(
                    property: sortFieldName,
                    direction: (sortDirection == SortDirection.Ascending) ? "ASC" : "DESC").ToList();

            if (pageSize != -1)
                records = records
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList();

            toReturn.AddRange(records);

            return toReturn;
        }

        public T Insert<T>(T entity) where T : class
        {
            return base.Set<T>()
                .Add(entity);
        }


        public void Update<T, K>(Expression<Func<T, bool>> selector, Expression<Func<T, K>> updatedEntity) where T : class
        {
            var entitiesToUpdate = base.Set<T>()
                               .Where(selector)
                               .ToList();

            if (entitiesToUpdate.Count > 0)
            {
                var compiledExpression = updatedEntity.Compile();
                var upatedFields = compiledExpression(null);

                foreach (var entityToUpdate in entitiesToUpdate)
                {
                    var entity = CopyProperties(upatedFields, entityToUpdate);
                    base.Entry(entity).State = System.Data.EntityState.Modified;
                }
            }
        }

        public K CopyProperties<T, K>(T source, K destination)
        {
            var s = source.GetType();
            var sourceProperties = s.GetProperties();

            var d = destination.GetType();
            var destinationProperties = d.GetProperties();

            foreach (PropertyInfo sp in sourceProperties)
            {
                var dp =
                    destinationProperties.SingleOrDefault(
                    x => x.Name == sp.Name && 
                        x.PropertyType == sp.PropertyType);

                if (dp != null && dp.CanWrite)
                    dp.SetValue(destination, sp.GetValue(source, null), null);
            }

            return destination;
        }

        [Obsolete("UpdateEntity<T> has been deprecated.  Please use Update<T, K> instead.", true)]
        public void UpdateEntity<T>(T entity) where T : class
        {
        }
    }
}
