using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.ServiceModel
{
    [DataContract]
    public class RequestBase
    {
        [DataMember]
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
