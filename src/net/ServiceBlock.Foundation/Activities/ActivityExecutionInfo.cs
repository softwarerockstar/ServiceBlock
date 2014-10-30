using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    public class ActivityExecutionInfo
    {
        public string DefaultWorkflowAssembly { get; private set; }
        public string DefaultWorkflowNamespace { get; private set; }
        public string DefaultValidationRuleSetName { get; private set; }
        public bool NullRequestCheckEnabled { get; private set; }
        public IList<ActivityExecutionOverride> ActivityExecutionOverrides { get; private set; }
        public IList<RequestValidationOverride> RequestValidationOverrides { get; private set; }

        public ActivityExecutionInfo(
            string defaultWorkflowAssembly, 
            string defaultWorkflowNamespace, 
            string defaultValidationRuleSetName,
            bool nullRequestCheckEnabled,
            string[] workflowOverrides,
            string[] validationOverrides)
        {

            Contract.Requires<ArgumentNullException>(
                !String.IsNullOrEmpty(defaultWorkflowAssembly),
                "defaultWorkflowAssembly parameter can't be null");


            Contract.Requires<ArgumentNullException>(
                !String.IsNullOrEmpty(defaultWorkflowNamespace),
                "defaultWorkflowNamespace parameter can't be null");

            this.DefaultWorkflowAssembly = defaultWorkflowAssembly;
            this.DefaultWorkflowNamespace = defaultWorkflowNamespace;
            this.DefaultValidationRuleSetName = defaultValidationRuleSetName;
            this.NullRequestCheckEnabled = nullRequestCheckEnabled;
            this.ActivityExecutionOverrides = GetExecutionOverrides(workflowOverrides);
            this.RequestValidationOverrides = GetValidationOverrides(validationOverrides);
        }

        private IList<RequestValidationOverride> GetValidationOverrides(string[] overrides)
        {
            var matches = overrides.Select(s => Regex.Match(s, @"entityName[\s]*=[\s]*(?<entityName>[\w\.]+);[\s]*operationName[\s]*=[\s]*(?<operationName>[\w\.]+);[\s]*rulesetName[\s]*=[\s]*(?<rulesetName>[\w\.]+)"));

            Contract.Assert(
                matches.All(s => s.Success),
                "Overrides are not specified in the correct format");

            var items = new List<RequestValidationOverride>();

            foreach (var match in matches)
            {
                items.Add(new RequestValidationOverride
                (
                    match.Groups["entityName"].Value,
                    match.Groups["operationName"].Value,
                    match.Groups["rulesetName"].Value
                ));
            }

            return items;
        }

        private IList<ActivityExecutionOverride> GetExecutionOverrides(string[] overrides)
        {
            var matches = overrides.Select(s => Regex.Match(s, @"entityName[\s]*=[\s]*(?<entityName>[\w\.]+);[\s]*operationName[\s]*=[\s]*(?<operationName>[\w\.]+);[\s]*type[\s]*=[\s]*(?<type>[\w\.]+[\s]*[^,]*,[\s]*[\w\.]+)"));
            
            Contract.Assert(
                matches.All(s => s.Success),
                "Overrides are not specified in the correct format");

            var items = new List<ActivityExecutionOverride>();

            foreach (var match in matches)
            {
                items.Add(new ActivityExecutionOverride
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
