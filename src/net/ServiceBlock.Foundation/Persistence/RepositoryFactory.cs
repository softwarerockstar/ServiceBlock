using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMediaStore.EnterpriseFramework.Persistence
{
    public static class RepositoryFactory         
    {
        public static IGenericRepository<T> GetRepository<T>() where T : class
        {
            object context = null;  // resolve context
            return GetRepository<T>(context);
        }

        public static IGenericRepository<T> GetRepository<T>(object context) where T : class
        {
            return new DbRepository<T>(context);
        }
    }
}
