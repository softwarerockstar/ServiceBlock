using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation;
using VHA.ServiceFoundation.Data;
using Cassandra;
using Cassandra.Data.Linq;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace VHA.ServiceFoundation.DataProviders
{
    public abstract class CassandraUnitOfWorkBase : IUnitOfWorkEx, IDisposable
    {
        private Session _session;
        private int _batchCount;
        private Batch _batch;

        public CassandraUnitOfWorkBase(string nameOrConnectionString)
        {
            object contactPoints = null;
            object port = null;
            object keyspaceName = null;
            object userName = null;
            object password = null;

            var match = Regex.Match(
                nameOrConnectionString,
                @"^name([\s]+)?=([\s]+)?(?<name>.*)", RegexOptions.IgnoreCase);

            string connectionString = (match.Success)
                                          ? ConfigurationManager.ConnectionStrings[match.Groups["name"].Value].ConnectionString
                                          : nameOrConnectionString;

            var connectionBuilder = new OleDbConnectionStringBuilder(connectionString);

            connectionBuilder.TryGetValue("Contact Points", out contactPoints);

            if (contactPoints == null)
                throw new ArgumentException(
                    "Invalid connection string. [Contact Points] must be specified.",
                    "nameOrConnectionString");

            connectionBuilder.TryGetValue("Port", out port);

            if (port == null)
                throw new ArgumentException(
                    "Invalid connection string. [Port] must be specified.",
                    "nameOrConnectionString");

            connectionBuilder.TryGetValue("Keyspace", out keyspaceName);

            if (keyspaceName == null)
                throw new ArgumentException(
                    "Invalid connection string. [Keyspace] must be specified.",
                    "nameOrConnectionString");
            
            connectionBuilder.TryGetValue("Username", out userName);
            connectionBuilder.TryGetValue("Password", out password);

            var clusterBuilder = Cluster.Builder()
                .WithConnectionString(String.Format("Contact Points={0};Port={1};", contactPoints, port));

            if (userName != null && password != null)
                clusterBuilder = clusterBuilder.WithCredentials(userName.ToString(), password.ToString());

            var cluster = clusterBuilder.Build();

            _session = cluster.Connect(keyspaceName.ToString());
            _batch = _session.CreateBatch();
        }

        public void Delete<T>(Expression<Func<T, bool>> selector) where T : class
        {
            var table = _session.GetTable<T>();
            _batch.Append(table.Where(selector).Delete());
        }

        public IEnumerable<T> GetAll<T>(string includeProperties = null) where T : class
        {
            var table = _session.GetTable<T>();
            return table.Execute().ToList();
        }

        public IEnumerable<T> GetWithSelector<T>(
            Expression<Func<T, bool>> selector,
            string includeProperties = null) where T : class
        {
            var table = _session.GetTable<T>();
            var toReturn = table
                .Where(selector)
                .Select(m => m)
                .Execute()
                .ToList();

            return toReturn;
        }

        public long GetCount<T>(Expression<Func<T, bool>> selector) where T : class
        {
            var table = _session.GetTable<T>();
            return table.Where(selector).Count().Execute();
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
            var query = _session.GetTable<T>() as CqlQuery<T>;

            foreach (var selector in selectors)
                query = query.Where(selector);

            var records = query
                            .Select(m => m)
                            .Execute()
                            .ToList();

            toReturn.TotalCount = records.Count;

            if (sortFieldName != null)
                records = records.AsQueryable<T>().OrderBy(
                    property: sortFieldName,
                    direction: (sortDirection == SortDirection.Ascending) ? "ASC" : "DESC").ToList();

            if (pageSize != -1)
                records = records
                    .Skip(pageSize*(pageNumber - 1))
                    .Take(pageSize)
                    .ToList();


            toReturn.AddRange(records);

            return toReturn;
        }

        public PagingList<T> GetWithCriteria<T>(Criteria criteria, string includeProperties=null) where T : class
        {
            var toReturn = new PagingList<T>();
            var table = _session.GetTable<T>();
            var queryBuilder = table as CqlQuery<T>;

            if (criteria.FilterFieldName != null && criteria.FilterFieldValue != null)
            {
                queryBuilder = queryBuilder.Where(
                    DynamicExpressionBuilder.BuildFilterExpression<T>(
                        criteria.FilterFieldName,
                        criteria.FilterFieldValue,
                        StringFilterOperator.Equals));
            }
            
            var select = queryBuilder.Select(m => m);
            var expression = select.Expression;

            Type d1 = typeof(CqlQuery<>);
            Type[] typeArgs = { typeof(T) };
            Type typeToInstantiate = d1.MakeGenericType(typeArgs);


            var cqlQuery = Activator.CreateInstance(typeToInstantiate,
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance,
                            null, new object[] { expression, table }, null) as CqlQuery<T>;
            
            var records = cqlQuery.Execute().ToList();
            toReturn.TotalCount = records.Count;

            if (criteria.SortFieldName != null)
                records = records.AsQueryable<T>().OrderBy(
                    property: criteria.SortFieldName,
                    direction: (criteria.SortDirection == SortDirection.Ascending) ? "ASC" : "DESC").ToList();

            if (criteria.PageSize != -1)
                records = records
                    .Skip(criteria.PageSize*(criteria.PageNumber - 1))
                    .Take(criteria.PageSize)
                    .ToList();


            toReturn.AddRange(records);

            return toReturn;
        }

        public T Insert<T>(T entity) where T : class
        {
            var table = _session.GetTable<T>();
            _batch.Append(table.Insert(entity));
            return entity;
        }

        public void Update<T, K>(Expression<Func<T, bool>> selector, Expression<Func<T, K>> updatedEntity) where T : class
        {
            var table = _session.GetTable<T>();
            _batch.Append(table.Where(selector).Select(updatedEntity).Update());
        }

        public int SaveChanges()
        {
            _batch.Execute();
            var toReturn = _batchCount;
            _batchCount = 0;

            return toReturn;
        }

        public IQueryable<T> GetEntitySet<T>() where T : class
        {
            return _session.GetTable<T>();
        }

        //public IEnumerable<T> GetWithEntityQuery<T>(
        //    IQueryable<T> query,
        //    string includeProperties = null) where T : class
        //{
        //    var table = _session.GetTable<T>();
        //    var expression = query.Expression;

        //    Type d1 = typeof (CqlQuery<>);
        //    Type[] typeArgs = {typeof (T)};
        //    Type typeToInstantiate = d1.MakeGenericType(typeArgs);


        //    var cqlQuery = Activator.CreateInstance(typeToInstantiate,
        //                    System.Reflection.BindingFlags.NonPublic |
        //                    System.Reflection.BindingFlags.Instance,
        //                    null, new object[] {expression, table}, null) as CqlQuery<T>;
            
        //    return cqlQuery.Execute();
        //}


        [Obsolete("UpdateEntity<T> has been deprecated.  Please use Update<T, K> instead.", true)]
        public void UpdateEntity<T>(T entity) where T : class
        {
        }


        #region IDisposable Implementation
        ~CassandraUnitOfWorkBase()
        {
            // the finalizer also has to release unmanaged resources,
            // in case the developer forgot to dispose the object.
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            // this tells the garbage collector not to execute the finalizer
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // clean up managed resources:

                // dispose child objects that implement IDisposable
                _session.Cluster.Shutdown();
                _session.Cluster.Dispose();
                _session.Dispose();
                _session = null;
            }

            // clean up unmanaged resources

        }
        #endregion
        
    }
}
