using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace ServiceBlock.Foundation.ServiceModel.BindingExtensions
{
        public class AssertEncryptionHttpTransportBindingElement: HttpTransportBindingElement
        {
            public AssertEncryptionHttpTransportBindingElement()
            {
            }

            public AssertEncryptionHttpTransportBindingElement(AssertEncryptionHttpTransportBindingElement elementToBeCloned)
                : base(elementToBeCloned)
            {
            }

            public override BindingElement Clone()
            {
                return new AssertEncryptionHttpTransportBindingElement(this);
            }

            public override T GetProperty<T>(BindingContext context)
            {
                if (typeof(T) == typeof(ISecurityCapabilities))
                {
                    return (T)(object)new AssertEncryptionSecurityCapabilities();
                }
                return base.GetProperty<T>(context);
            }

            public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
            {
                return base.BuildChannelFactory<TChannel>(context);
            }
        }
}
