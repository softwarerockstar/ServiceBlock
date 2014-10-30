using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VHA.ServiceFoundation;

namespace VHA.ServiceFoundation.Data
{
    public abstract class CassandraAdoUnitOfWorkBase : IAdoNetUnitOfWork
    {
        private int _commitCount;
        private DbConnection _connection;

        public CassandraAdoUnitOfWorkBase(string nameOrConnectionString)
        {
            object contactPoints = null;
            object port = null;
            object keyspaceName = null;

            var provider = DbProviderFactories.GetFactory("Cassandra.Data.CqlProviderFactory");

            var match = Regex.Match(
                nameOrConnectionString,
                @"^name([\s]+)?=([\s]+)?(?<name>.*)", RegexOptions.IgnoreCase);

            string connectionString = (match.Success)
                                          ? ConfigurationManager.ConnectionStrings[match.Groups["name"].Value].ConnectionString
                                          : nameOrConnectionString;

            var builder = provider.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionString;

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

            _connection = provider.CreateConnection();
            _connection.ConnectionString = String.Format("Contact Points={0};Port={1};", contactPoints, port);
            _connection.Open();

            _connection.ChangeDatabase(keyspaceName.ToString());
        }

        public IList<T> ExecuteQuery<T>(string query) where T : class, new()
        {
            List<T> toReturn = new List<T>();

            var command = _connection.CreateCommand();
            command.CommandText = query;

            toReturn = command
                .ExecuteReader()
                .To<T>()
                .ToList();

            return toReturn;
        }

        public void ExecuteNonQuery(string query)
        {
            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();

            _commitCount++;
        }


        public int SaveChanges()
        {
            return _commitCount;
        }

        public void UpdateEntity<T>(T entity) where T : class
        {
        }


        #region IDisposable Implementation
        ~CassandraAdoUnitOfWorkBase()
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
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }

            // clean up unmanaged resources

        }
        #endregion
    }
}
