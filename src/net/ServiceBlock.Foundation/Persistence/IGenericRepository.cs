// SOURCE: http://www.martinwilley.com/net/code/data/dbcontextextensions.html
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMediaStore.EnterpriseFramework.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Get();
        IQueryable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties);
        T Find(IList<object> keyValues);
        void Add(T entity);
        void Update(T entity);
        void AddOrUpdate(T entity);
        void Delete(IList<object> keyValues);
        int SaveChanges();
    }
}
