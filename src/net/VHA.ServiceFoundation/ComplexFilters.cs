using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace VHA.ServiceFoundation
{
   
    public class ComplexFilters : List<ComplexFilter>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            this.ForEach(s => sb.AppendFormat("{{0}}", s.ToString()));
            sb.Append("]");

            return sb.ToString();
        }

        public static List<ComplexFilter> FromString(string source)
        {
            var toReturn = new ComplexFilters();
            //?complexFilters=[{FilterField:"xyz",FilterValue:"abc",Operator:"="}]
            if (source != null)
            {
                return new JavaScriptSerializer().Deserialize<ComplexFilters>(source);
            }

            return toReturn;

        }
    }
}
