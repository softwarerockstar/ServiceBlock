using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder.Models
{
    [Serializable]
    public class ModelContext
    {
        public IList<PropertyInfo> Properties { get; set; }

        public IDictionary<string, object> ExtendedProperties { get; set; }
    }
}
