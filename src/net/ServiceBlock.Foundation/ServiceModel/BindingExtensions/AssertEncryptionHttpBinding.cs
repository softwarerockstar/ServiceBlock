using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Configuration;
using System.Xml;

namespace ServiceBlock.Foundation.ServiceModel.BindingExtensions
{
    public class AssertEncryptionHttpBinding : CustomBinding, IBindingRuntimePreferences
    {
        public AssertEncryptionHttpSecurityMode SecurityMode { get; set; }
        public WSMessageEncoding MessageEncoding {get; set;}

        public new MessageVersion MessageVersion
        {
            get
            {
                if (this.MessageEncoding == WSMessageEncoding.Text)
                    return this.TextEncoding.MessageVersion;
                else
                    return this.MtomEncoding.MessageVersion;
            }
            set
            {
                this.TextEncoding.MessageVersion = value;
                this.MtomEncoding.MessageVersion = value;
            }
        }

        public long MaxReceivedMessageSize
        {
            get
            {
                return this.Transport.MaxReceivedMessageSize;
            }
            set
            {
                this.Transport.MaxReceivedMessageSize = value;
            }
        }

        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {

                if (this.MessageEncoding == WSMessageEncoding.Text)
                    return this.TextEncoding.ReaderQuotas;
                else
                    return this.MtomEncoding.ReaderQuotas;
            }
            set
            {
                value.CopyTo(this.TextEncoding.ReaderQuotas);
                value.CopyTo(this.MtomEncoding.ReaderQuotas);
            }
        }

        private AssertEncryptionHttpTransportBindingElement Transport { get; set; }
        private TextMessageEncodingBindingElement TextEncoding { get; set; }
        private MtomMessageEncodingBindingElement MtomEncoding { get; set; }


 
        public AssertEncryptionHttpBinding()
            : this(AssertEncryptionHttpSecurityMode.None)
        {
        }

        public AssertEncryptionHttpBinding(AssertEncryptionHttpSecurityMode securityMode)
        {
            
            this.SecurityMode = securityMode;

            this.MessageEncoding = WSMessageEncoding.Text;
            this.TextEncoding = new TextMessageEncodingBindingElement();
            this.MtomEncoding = new MtomMessageEncodingBindingElement();
            this.MessageVersion= MessageVersion.Soap12WSAddressing10;

            this.Transport = new AssertEncryptionHttpTransportBindingElement();
        }


        public AssertEncryptionHttpBinding(string configurationName): this()
        {
            var bindingElement = AssertEncryptionHttpBindingCollectionElement.GetBindingCollectionElement().Bindings[configurationName] as AssertEncryptionHttpBindingElement;

            if (bindingElement == null)
                throw new ConfigurationErrorsException(string.Format("Binding configuration element missing: {0} in {1}", configurationName, AssertEncryptionHttpBindingCollectionElement.BindingCollectionElementName));

            bindingElement.ApplyConfiguration(this);
        }

        public override BindingElementCollection CreateBindingElements()
        {
            var bindingElements = new BindingElementCollection();

            // if passing credentials via message security, add a security element
            TransportSecurityBindingElement transportSecurityElement = null;
            
            if (this.SecurityMode == AssertEncryptionHttpSecurityMode.UserNameOverMessage)
            {
                transportSecurityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            }

            if (this.SecurityMode == AssertEncryptionHttpSecurityMode.UserNameOverTransport)
            {
                transportSecurityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            }

            if (transportSecurityElement != null)
                bindingElements.Add(transportSecurityElement);

            // add a message encoder element
            //if (this.MessageEncoding == WSMessageEncoding.Text)
            //    bindingElements.Add(this.TextEncoding);
            //else if (this.MessageEncoding == WSMessageEncoding.Mtom)
            //    bindingElements.Add(this.MtomEncoding);

            // add a transport element
            bindingElements.Add(this.GetTransport());

            return bindingElements;
        }

        private TransportBindingElement GetTransport()
        {
            //if (this.SecurityMode == AssertEncryptionHttpSecurityMode.UserNameOverTransport)
            //{
            //    this.Transport.AuthenticationScheme = System.Net.AuthenticationSchemes.Basic;
            //}
            return this.Transport;
        }

        public override string Scheme
        {
            get { return this.Transport.Scheme; }
        }
        
        #region IBindingRuntimePreferences Members

        public bool ReceiveSynchronously
        {
            get { return false; }
        }

        #endregion
    }
}
