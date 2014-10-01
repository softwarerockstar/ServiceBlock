using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;

namespace VHA.ServiceFoundation
{
    [DataContract]
    public class ComplexFilter
    {
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string FieldValue { get; set; }
        [DataMember]
        public string Operator { get; set; }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

        public static List<ComplexFilter> FromJson(string source)
        {
            var toReturn = new List<ComplexFilter>();

            if (source != null)
                toReturn = new JavaScriptSerializer().Deserialize<List<ComplexFilter>>(source);

            return toReturn;
        }

        public static string ToJson(IList<ComplexFilter> filters)
        {
            string toReturn = null;

            if (filters != null)
                toReturn = new JavaScriptSerializer().Serialize(filters);

            return toReturn;
        }

    }
}
