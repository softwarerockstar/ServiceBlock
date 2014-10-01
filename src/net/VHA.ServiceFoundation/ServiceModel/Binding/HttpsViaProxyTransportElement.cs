#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;
#endregion

namespace VHA.ServiceFoundation.ServiceModel.Binding
{
    /// <summary>
    /// HttpsViaProxyTransportElement
    /// </summary>
    public class HttpsViaProxyTransportElement : HttpTransportElement
    {
        /// <summary>
        /// Gets the type of binding.
        /// </summary>
        /// <value></value>
        /// <returns>The type of binding.</returns>
        public override Type BindingElementType
        {
            get
            {
                return typeof(HttpsViaProxyTransportBindingElement);
            }
        }

        /// <summary>
        /// Creates a binding element from the settings in this configuration element.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.TransportBindingElement"/> whose properties are copied from the settings in this configuration element.
        /// </returns>
        protected override TransportBindingElement CreateDefaultBindingElement()
        {
            HttpsViaProxyTransportBindingElement element = new HttpsViaProxyTransportBindingElement();
            //element.AuthenticationScheme = AuthenticationSchemes.Negotiate;
            //element.UnsafeConnectionNtlmAuthentication = true;
            //return new HttpsViaProxyTransportBindingElement(); 
            return element;
        }

        //public static TransportSecurityBindingElement CreateSspiNegotiationOverTransportBindingElement()
        //{
        //    TransportSecurityBindingElement element = new TransportSecurityBindingElement();

        //    return element;
        //}

    } 

}
