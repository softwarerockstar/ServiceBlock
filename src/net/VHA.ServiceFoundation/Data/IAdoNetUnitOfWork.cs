using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Data
{
    public interface IAdoNetUnitOfWork : IUnitOfWork, IDisposable
    {
        IList<T> ExecuteQuery<T>(string query) where T : class, new();
        void ExecuteNonQuery(string query);
    }
}
