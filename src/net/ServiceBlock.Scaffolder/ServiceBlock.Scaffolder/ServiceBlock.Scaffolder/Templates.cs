using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder
{
    public static class Templates
    {
        public static string[] MessageTemplates = new string[] 
        { 
            "AddOrUpdateRequest",
            "AddOrUpdateResponse",
            "DeleteRequest",
            "DeleteResponse",
            "GetAllRequest",
            "GetAllResponse",
            "GetByIdRequest",
            "GetByIdResponse",
            "GetWithCriteriaRequest",
            "GetWithCriteriaResponse"
        };

        public static string[] UoWTemplates = new string[]
        {
            "EfDefaultUnitOfWork"
        };

        public static string[] DataActivityTemplates = new string[] 
        { 
            "Delete",
            "GetAll",
            "GetById",
            "GetWithCriteria",
            "InsertOrUpdate"
        };

        public static string[] OrchestrationTemplates = new string[] 
        { 
            "AddOrUpdate",
            "Delete",
            "GetAll",
            "GetById",
            "GetWithCriteria"           
        };

        public static string[] WebApiTemplates = new string[] 
        { 
            "Controller"      
        };

        public static string[] WebUIControllerTemplates = new string[] 
        { 
            "Controller"
        };

        public static string[] WebUIViewTemplates = new string[] 
        { 
            "View"
        };        


    }
}
