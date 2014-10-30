#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
#endregion

namespace ServiceBlock.Foundation.ServiceModel.Binding
{
    /// <summary>
    /// HttpsViaProxyTransportBindingElement 
    /// </summary>
    public class HttpsViaProxyTransportBindingElement : HttpTransportBindingElement, ITransportTokenAssertionProvider
    {
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(ISecurityCapabilities))
            {
                return (T)(ISecurityCapabilities)new AutoSecuredHttpSecurityCapabilities();
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override BindingElement Clone()
        {
            return new HttpsViaProxyTransportBindingElement();
        }

        /// <summary>
        /// Gets the transport token assertion.
        /// </summary>
        /// <returns></returns>
        public System.Xml.XmlElement GetTransportTokenAssertion()
        {
            return null;
        }

    }

}
