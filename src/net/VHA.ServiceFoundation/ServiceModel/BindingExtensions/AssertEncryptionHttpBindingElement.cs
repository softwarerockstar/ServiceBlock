// © 2009 Michele Leroux Bustamante. All rights reserved 
// Book: Learning WCF, O'Reilly
// Book Blog: www.thatindigogirl.com
// Michele's Blog: www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VHA.ServiceFoundation.ServiceModel.BindingExtensions
{
    public enum AssertEncryptionHttpSecurityMode
    {
        None,
        UserNameOverTransport,
        UserNameOverMessage
    }

    public class AssertEncryptionHttpBindingElement: StandardBindingElement
    {
        [ConfigurationProperty("messageEncoding", DefaultValue = 0)]
        public WSMessageEncoding MessageEncoding 
        { 
            get
            {
                return (WSMessageEncoding)base["messageEncoding"];
            }

            set
            {
                base["messageEncoding"] = value.ToString();
            }

        }
        
        [ConfigurationProperty("maxReceivedMessageSize", DefaultValue = 0x10000L), LongValidator(MinValue = 1L)]
        public long MaxReceivedMessageSize 
        {
            get
            {
                return (long)base["maxReceivedMessageSize"];
            }

            set
            {
                base["maxReceivedMessageSize"] = value;
            }
    
        }

        public const string DefaultMessageVersion = "Soap11WSAddressing10";

        [ConfigurationProperty("messageVersion", DefaultValue = AssertEncryptionHttpBindingElement.DefaultMessageVersion)]
        public MessageVersion MessageVersion 
        { 
            get
            {
                var messageVersion = (string)base["messageVersion"];
                System.Reflection.PropertyInfo propertyInfo = typeof(MessageVersion).GetProperty(messageVersion);
                return (MessageVersion)propertyInfo.GetValue(null, null);
            }
            set
            {
                base["messageVersion"] = value.ToString();
            }
        }

        [ConfigurationProperty("readerQuotas")]
        public XmlDictionaryReaderQuotasElement ReaderQuotas
        {
            get
            {
                return (XmlDictionaryReaderQuotasElement)base["readerQuotas"];
            }
        }

        [ConfigurationProperty("securityMode", DefaultValue = AssertEncryptionHttpSecurityMode.None)]
        public AssertEncryptionHttpSecurityMode SecurityMode 
        {
            get
            {
                return (AssertEncryptionHttpSecurityMode)base["securityMode"];
            }

            set
            {
                base["securityMode"] = value;
            }
        }

        protected override Type BindingElementType
        {
            get { return typeof(AssertEncryptionHttpBinding); }
        }

        protected override void InitializeFrom(System.ServiceModel.Channels.Binding binding)
        {
            base.InitializeFrom(binding);

            var assertEncryptionHttpBinding = (AssertEncryptionHttpBinding)binding;
            this.MessageEncoding = assertEncryptionHttpBinding.MessageEncoding;
            this.MessageVersion = assertEncryptionHttpBinding.MessageVersion;
            this.MaxReceivedMessageSize = assertEncryptionHttpBinding.MaxReceivedMessageSize;
            this.SecurityMode = assertEncryptionHttpBinding.SecurityMode;
        }

        protected override void OnApplyConfiguration(System.ServiceModel.Channels.Binding binding)
        {
            var assertEncryptionHttpBinding = (AssertEncryptionHttpBinding)binding;
            assertEncryptionHttpBinding.MessageEncoding = this.MessageEncoding;
            assertEncryptionHttpBinding.MessageVersion = this.MessageVersion;
            assertEncryptionHttpBinding.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            
            if (this.ReaderQuotas != null)
            {
                if (this.ReaderQuotas.MaxDepth != 0)
                {
                    assertEncryptionHttpBinding.ReaderQuotas.MaxDepth = this.ReaderQuotas.MaxDepth;
                }
                if (this.ReaderQuotas.MaxStringContentLength != 0)
                {
                    assertEncryptionHttpBinding.ReaderQuotas.MaxStringContentLength = this.ReaderQuotas.MaxStringContentLength;
                }
                if (this.ReaderQuotas.MaxArrayLength != 0)
                {
                    assertEncryptionHttpBinding.ReaderQuotas.MaxArrayLength = this.ReaderQuotas.MaxArrayLength;
                }
                if (this.ReaderQuotas.MaxBytesPerRead != 0)
                {
                    assertEncryptionHttpBinding.ReaderQuotas.MaxBytesPerRead = this.ReaderQuotas.MaxBytesPerRead;
                }
                if (this.ReaderQuotas.MaxNameTableCharCount != 0)
                {
                    assertEncryptionHttpBinding.ReaderQuotas.MaxNameTableCharCount = this.ReaderQuotas.MaxNameTableCharCount;
                }
                
            }

            assertEncryptionHttpBinding.SecurityMode = this.SecurityMode;
        }

        private ConfigurationPropertyCollection m_properties;

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {

                ConfigurationPropertyCollection properties = null;
                if (this.m_properties == null)
                {
                    properties = base.Properties;

                    properties.Add(new ConfigurationProperty("messageEncoding", typeof(WSMessageEncoding), WSMessageEncoding.Text, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("maxReceivedMessageSize", typeof(long), 0x10000L, null, new LongValidator(1L, 0x7fffffffffffffffL, false), ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("messageVersion", typeof(string), AssertEncryptionHttpBindingElement.DefaultMessageVersion, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("readerQuotas", typeof(XmlDictionaryReaderQuotasElement), null, null, null, ConfigurationPropertyOptions.None));
                    properties.Add(new ConfigurationProperty("securityMode", typeof(AssertEncryptionHttpSecurityMode), AssertEncryptionHttpSecurityMode.None, null, null, ConfigurationPropertyOptions.None));

                    this.m_properties = properties;
                }

                return this.m_properties;
            }
        }
    }
}
