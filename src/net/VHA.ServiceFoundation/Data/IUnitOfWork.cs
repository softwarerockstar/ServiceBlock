using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Data
{
    public interface IUnitOfWork
    {
        void UpdateEntity<T>(T entity) where T : class;
        int SaveChanges();
    }
}
