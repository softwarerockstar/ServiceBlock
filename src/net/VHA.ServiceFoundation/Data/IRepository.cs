using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMediaStore.EnterpriseFramework.Data
{
    public interface IRepository<T> : IDbSet<T> where T:class
    {
        //T Find(params object[] keyValues);

        //T Add(T item);

        //T Remove(T item);

        //T Attach(T item);

        //T Detach(T item);

        //T Create();

        //TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T;

        //ObservableCollection<T> Local {get;}
    }
}
