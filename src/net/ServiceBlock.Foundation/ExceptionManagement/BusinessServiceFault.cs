using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation.ExceptionManagement
{
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class BusinessServiceFault
    {
        string _html;
        string _message;
        Guid _errorId;

        [DataMember]
        public string Message 
        {
            get { return _message; }
            set { _message = value; }
        }

        [DataMember]
        public Guid ErrorID
        {
            get { return _errorId; }
            set { _errorId = value; }
        }


        [DataMember]
        public string Html
        {
            get
            {
                if (_html == null)
                {
                    _html = String.Format("Error ID:{0}", _errorId);
                }

                return _html;
            }

            set { _html = value; }
        }
    }
}
