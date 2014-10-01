using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation.ServiceModel
{
    [DataContract]
    public class ResponseBase
    {
        [DataMember]
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
