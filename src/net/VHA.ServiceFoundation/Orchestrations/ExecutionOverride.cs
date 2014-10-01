using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation.Orchestrations
{
    public class ExecutionOverride
    {        
        public string EntityName { get; private set; }
        public string OperationName { get; private set; }     
        public Type WorkflowType { get; private set; }

        public ExecutionOverride(string entityName, string operationName, string type)
        {            
            Contract.Ensures(
                this.WorkflowType != null,
                "Type specified in overrides constructor paramter of ActivityExecutionInfo could not be loaded.");

            this.EntityName = entityName;
            this.OperationName = operationName;

            var typeParts = type.Split(',');

            var assembly = Assembly.Load(typeParts[1].Trim());

            this.WorkflowType = assembly
                .GetTypes()
                .FirstOrDefault(s => s.FullName == typeParts[0].Trim());
        }
    }
}
