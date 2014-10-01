using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace VHA.ServiceFoundation.ServiceModel.BindingExtensions
{
    class AssertEncryptionHttpTransportElement: HttpTransportElement
    {
        public override void ApplyConfiguration(System.ServiceModel.Channels.BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
        }

        public override Type BindingElementType
        {
            get
            {
                return typeof(AssertEncryptionHttpTransportBindingElement);
            }
        }

        protected override System.ServiceModel.Channels.BindingElement CreateBindingElement()
        {
            return base.CreateBindingElement();
        }

        protected override System.ServiceModel.Channels.TransportBindingElement CreateDefaultBindingElement()
        {
            return new AssertEncryptionHttpTransportBindingElement();
        }

    }
}
