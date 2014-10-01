using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace VHA.ServiceFoundation.ServiceModel.BindingExtensions
{
public class AssertEncryptionHttpBindingCollectionElement: StandardBindingCollectionElement<AssertEncryptionHttpBinding,AssertEncryptionHttpBindingElement>
{

    public const string BindingCollectionElementName = "assertEncryptionHttpBinding";

     public static AssertEncryptionHttpBindingCollectionElement GetBindingCollectionElement()
    {
        AssertEncryptionHttpBindingCollectionElement bindingCollectionElement = null;

        BindingsSection bindingsSection = ConfigurationManager.GetSection("system.serviceModel/bindings") as BindingsSection;
        if (bindingsSection != null)
        {
            bindingCollectionElement = bindingsSection[AssertEncryptionHttpBindingCollectionElement.BindingCollectionElementName] as AssertEncryptionHttpBindingCollectionElement;
        }
        
        return bindingCollectionElement;
    }
   
}

    
}
