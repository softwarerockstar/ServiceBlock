using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation.Properties;

namespace VHA.ServiceFoundation.ExceptionManagement
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BusinessException : ApplicationException
    {
        public string FriendlyMessage { get; private set; }

        public BusinessException(string message, Exception innerException, string friendlyMessage)
            : base(message, innerException)
        {
            if (friendlyMessage == null)
                this.FriendlyMessage = Resources.BusinessExceptionDefaultFriendlyMessage;
            else
                this.FriendlyMessage = friendlyMessage;
        }

        public BusinessException(Exception innerException, string friendlyMessage)
            : this(null, innerException, friendlyMessage)
        {
        }

        public BusinessException(Exception innerException)
            : this(null, innerException, null)
        {
        }


        public BusinessException(string message, string friendlyMessage)
            : this(message, null, friendlyMessage)
        {
        }

        public BusinessException(string message, Exception innerException)
            : this(message, innerException, null)
        {
        }


        public BusinessException(string message)
            : this(message, null, null)
        {            
        }

        public BusinessException() : this(null, null, null)
        {            
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("FriendlyMessage", this.FriendlyMessage);
        }

    }
}
