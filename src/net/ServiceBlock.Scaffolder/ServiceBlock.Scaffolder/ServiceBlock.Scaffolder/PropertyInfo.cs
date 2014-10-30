using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder
{
    [Serializable]
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsNavigation { get; set; }
        public bool IsNonKey { get; set; }
    }
}
