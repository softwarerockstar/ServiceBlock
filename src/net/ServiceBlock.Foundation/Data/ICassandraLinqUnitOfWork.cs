using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra.Data.Linq;

namespace VHA.ServiceFoundation.Data
{
    public interface ICassandraLinqUnitOfWork : IDisposable, IUnitOfWork
    {
        IEnumerable<T> ExecuteQuery<T>(CqlQuery<T> query);
        void ExecuteNonQuery(CqlCommand command);
        void ExecutePassthroughCql(string cql);
        Table<T> GetKeyspace<T>() where T : class;
    }
}
