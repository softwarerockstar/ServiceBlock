using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.ExceptionManagement
{
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class ServiceValidationFault
    {
        private string _html;

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public IList<string> Details { get; set; }

        [DataMember]
        public string Html
        {
            get
            {
                if (_html == null)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    sb.Append("<ul>");

                    foreach (var detail in this.Details)
                        sb.AppendFormat("<li>{0}</li>", detail);

                    sb.Append("</ul>");

                    _html = sb.ToString();
                }
                return _html;
            }

            set { _html = value; }
        }

        public ServiceValidationFault(string message)
        {
            this.Message = message;
            this.Details = new List<string>();
        }

        public ServiceValidationFault()
            : this(null)
        {
        }
    }
}
