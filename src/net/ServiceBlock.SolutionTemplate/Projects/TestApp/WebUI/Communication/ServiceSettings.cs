using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public static class ServiceSettings
    {
        public static string ServiceBaseUri 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["serviceBlock:restEndpointUri"].ToString();
            }
 
        }

        public static string GetRestEndpointForEntity(string entity)
        {
            return ServiceBaseUri + entity;
        }
    }
}