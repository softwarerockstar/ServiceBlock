// SOURCE: http://www.martinwilley.com/net/code/data/dbcontextextensions.html
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DigitalMediaStore.EnterpriseFramework.Persistence
{
    public class DbRepository<T> : IGenericRepository<T> where T : class
    {
        readonly DbContext _context;

        public DbRepository(object context)
        {
            _context = context as DbContext;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Find(IList<object> keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void AddOrUpdate(T entity)
        {
            //uses DbContextExtensions to check value of primary key
            _context.AddOrUpdate(entity);
        }

        public void Delete(IList<object> keyValues)
        {
            //uses DbContextExtensions to attach a stub (or the actual entity if loaded)
            var stub = _context.Load<T>(keyValues);
            _context.Set<T>().Remove(stub);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
