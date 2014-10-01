using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Orchestrations
{
    public class OrchestrationExecutionInfo
    {
        public string DefaultWorkflowAssembly { get; private set; }
        public string DefaultWorkflowNamespace { get; private set; }
        public bool NullRequestCheckEnabled { get; private set; }
        public IList<ExecutionOverride> ExecutionOverrides { get; private set; }

        public OrchestrationExecutionInfo(
            string defaultWorkflowAssembly, 
            string defaultWorkflowNamespace, 
            bool nullRequestCheckEnabled,
            string[] executionOverrides)
        {

            if (String.IsNullOrEmpty(defaultWorkflowAssembly))
                throw new ArgumentNullException("defaultWorkflowAssembly");

            if (String.IsNullOrEmpty(defaultWorkflowNamespace))
                throw new ArgumentNullException("defaultWorkflowNamespace");

            this.DefaultWorkflowAssembly = defaultWorkflowAssembly;
            this.DefaultWorkflowNamespace = defaultWorkflowNamespace;
            this.NullRequestCheckEnabled = nullRequestCheckEnabled;
            this.ExecutionOverrides = GetExecutionOverrides(executionOverrides);
        }
 
        private IList<ExecutionOverride> GetExecutionOverrides(string[] overrides)
        {
            var matches = overrides.Select(s => Regex.Match(s, @"entityName[\s]*=[\s]*(?<entityName>[\w\.]+);[\s]*operationName[\s]*=[\s]*(?<operationName>[\w\.]+);[\s]*type[\s]*=[\s]*(?<type>[\w\.]+[\s]*[^,]*,[\s]*[\w\.]+)"));
            
            Contract.Assert(
                matches.All(s => s.Success),
                "Overrides are not specified in the correct format");

            var items = new List<ExecutionOverride>();

            foreach (var match in matches)
            {
                items.Add(new ExecutionOverride
                (
                    match.Groups["entityName"].Value,
                    match.Groups["operationName"].Value,
                    match.Groups["type"].Value
                ));
            }

            return items;
        }
    }
}
