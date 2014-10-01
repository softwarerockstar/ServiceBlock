using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;

namespace VHA.ServiceFoundation.Data
{
    public abstract class CassandraLinqDataContextBase : ICassandraLinqUnitOfWork
    {
        private Session _session;
        private int _batchCount;
        private Batch _batch;

        public CassandraLinqDataContextBase(string nameOrConnectionString)
        {
            object contactPoints = null;
            object port = null;
            object keyspaceName = null;

            var match = Regex.Match(
                nameOrConnectionString,
                @"^name([\s]+)?=([\s]+)?(?<name>.*)", RegexOptions.IgnoreCase);

            string connectionString = (match.Success)
                                          ? ConfigurationManager.ConnectionStrings[match.Groups["name"].Value].ConnectionString
                                          : nameOrConnectionString;

            var builder = new OleDbConnectionStringBuilder(connectionString);

            builder.TryGetValue("Contact Points", out contactPoints);

            if (contactPoints == null)
                throw new ArgumentException(
                    "Invalid connection string. [Contact Points] must be specified.",
                    "nameOrConnectionString");

            builder.TryGetValue("Port", out port);

            if (port == null)
                throw new ArgumentException(
                    "Invalid connection string. [Port] must be specified.",
                    "nameOrConnectionString");

            builder.TryGetValue("Keyspace", out keyspaceName);

            if (keyspaceName == null)
                throw new ArgumentException(
                    "Invalid connection string. [Keyspace] must be specified.",
                    "nameOrConnectionString");

            var cluster = Cluster.Builder()
                .WithConnectionString(String.Format("Contact Points={0};Port={1};", contactPoints, port))
                .Build();

            _session = cluster.Connect(keyspaceName.ToString());
            _batch = _session.CreateBatch();
        }

        public Table<T> GetKeyspace<T>() where T : class
        {
            return _session.GetTable<T>();
        }

        public IEnumerable<T> ExecuteQuery<T>(CqlQuery<T> query)
        {
            if (query != null)
                return query.Execute();
            else
                return null;
        }

        public void ExecuteNonQuery(CqlCommand command)
        {
            _batch.Append(command);
        }

        public void ExecutePassthroughCql(string cql)
        {
            _session.Execute(cql);
        }

        public int SaveChanges()
        {
            _batch.Execute();
            var toReturn = _batchCount;
            _batchCount = 0;

            return toReturn;
        }

        public void UpdateEntity<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }


        #region IDisposable Implementation
        ~CassandraLinqDataContextBase()
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
