#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.Security;
#endregion

namespace ServiceBlock.Foundation.ServiceModel.Binding
{
    /// <summary>
    /// AutoSecuredHttpSecurityCapabilities
    /// </summary>
    public class AutoSecuredHttpSecurityCapabilities : ISecurityCapabilities
    {
        /// <summary>
        /// Gets the protection level requests supported by the binding.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Net.Security.ProtectionLevel"/> that specifies the protection level requests supported by the binding.</returns>
        public ProtectionLevel SupportedRequestProtectionLevel
        {
            get { return ProtectionLevel.EncryptAndSign; }
        }

        /// <summary>
        /// Gets the protection level responses supported by the binding.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Net.Security.ProtectionLevel"/> that specifies the protection level responses supported by the binding.</returns>
        public ProtectionLevel SupportedResponseProtectionLevel
        {
            get { return ProtectionLevel.EncryptAndSign; }
        }

        /// <summary>
        /// Gets a value that indicates whether the binding supports client authentication.
        /// </summary>
        /// <value></value>
        /// <returns>true if the binding can support client authentication; otherwise, false.</returns>
        public bool SupportsClientAuthentication
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value that indicates whether the binding supports client Windows identity.
        /// </summary>
        /// <value></value>
        /// <returns>true if the binding can support client Windows identity; otherwise, false.</returns>
        public bool SupportsClientWindowsIdentity
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value that indicates whether the binding supports server authentication.
        /// </summary>
        /// <value></value>
        /// <returns>true if the binding can support server authentication; otherwise, false.</returns>
        public bool SupportsServerAuthentication
        {
            get { return true; }
        }

    } 

}
