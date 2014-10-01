using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Net.Security;

namespace ServiceBlock.Foundation.ServiceModel.BindingExtensions
{
    public class AssertEncryptionSecurityCapabilities: ISecurityCapabilities
    {

        #region ISecurityCapabilities Members

        public ProtectionLevel SupportedRequestProtectionLevel
        {
            get { return ProtectionLevel.EncryptAndSign;  }
        }

        public ProtectionLevel SupportedResponseProtectionLevel
        {
            get { return ProtectionLevel.EncryptAndSign;  }
        }

        public bool SupportsClientAuthentication
        {
            get { return false; }
        }

        public bool SupportsClientWindowsIdentity
        {
            get { return false; }
        }

        public bool SupportsServerAuthentication
        {
            get { return true; }
        }

        #endregion
    }
}
