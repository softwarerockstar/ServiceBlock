using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Data
{
    public interface IUnitOfWorkEx : IUnitOfWork
    {
        void Delete<T>(Expression<Func<T, bool>> selector) where T : class;

        IQueryable<T> GetEntitySet<T>() where T : class;

        IEnumerable<T> GetAll<T>(string includeProperties = null) where T : class;
        
        IEnumerable<T> GetWithSelector<T>(
            Expression<Func<T, bool>> selector, 
            string includeProperties = null) where T : class;

        //IEnumerable<T> GetWithEntityQuery<T>(
        //    IQueryable<T> query,
        //    string includeProperties = null) where T : class;

        PagingList<T> GetWithCriteria<T>(
            Criteria criteria, 
            string includeProperties = null) where T : class;

        PagingList<T> GetWithCriteria<T>(
            IList<Expression<Func<T, bool>>> selectors,
            string sortFieldName = null,
            SortDirection sortDirection = SortDirection.Ascending,
            int pageSize = 10,
            int pageNumber = 1,
            string includeProperties = null) where T : class;

        long GetCount<T>(Expression<Func<T, bool>> selector) where T : class;
        
        T Insert<T>(T entity) where T : class;
       
        // Allows updating only specified field by passing in an anonymous type, e.g.
        // BulkUpdate<Category>( s => s.CategoryId = 1, w => new {IsActive = false} );
        void Update<T, K>(
            Expression<Func<T, bool>> selector,
            Expression<Func<T, K>> updatedEntity) where T : class;

    }
}
