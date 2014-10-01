using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.ServiceModel
{
    public class WcfOperationContext : IExtension<OperationContext>
    {
        private static readonly WcfOperationContext _current;
        private readonly IDictionary<string, object> _items;

        static WcfOperationContext()
        {
            _current = new WcfOperationContext();
            OperationContext.Current.Extensions.Add(_current);            
        }

        private WcfOperationContext()
        {
            _items = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Items
        {
            get { return _items; }
        }

        public static WcfOperationContext Current
        {
            get { return _current;  }
        }

        public void Attach(OperationContext owner) { }
        public void Detach(OperationContext owner) { }
    }
}
