using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    public class RequestValidationOverride
    {
        public string EntityName { get; private set; }
        public string OperationName { get; private set; }     
        public string ValidationRulesetName { get; private set; }

        public RequestValidationOverride(string entityName, string operationName, string rulesetName)
        {            
            this.EntityName = entityName;
            this.OperationName = operationName;
            this.ValidationRulesetName = rulesetName;
        }
    }
}
