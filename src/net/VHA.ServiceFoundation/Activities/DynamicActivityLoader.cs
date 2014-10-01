using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    //TODO: Should this cache workflow instance or definition?
    public static class DynamicActivityLoader
    {
        public static K GetMe<T, K>(string activityPartialName, T request, ValidatorFactory validatorFactory, DbContext dbContext)
            where K : class
            where T : class
        {
            string worflowAssemblyName = "DigitalMediaStore.Orchestrations";
            var workflowAssembly = Assembly.Load(worflowAssemblyName);
            var activityType = workflowAssembly.GetType(String.Format("{0}.Workflows.Custom{1}", worflowAssemblyName, activityPartialName));

            if (activityType == null)
                activityType = workflowAssembly.GetType(String.Format("{0}.Workflows.{1}", worflowAssemblyName, activityPartialName));

            var validator = validatorFactory.CreateValidator(typeof(T),
                ConfigurationManager.AppSettings["DefaultValidationRuleSetName"]);

            Dictionary<string, object> arguments = new Dictionary<string, object>
            {
                { "request", request },
                { "validator", validator },
                { "dbContext", dbContext }
            };

            var activity = Activator.CreateInstance(activityType) as Activity;
            var results = WorkflowInvoker.Invoke(activity, arguments);

            return results["response"] as K;
        }

        static Dictionary<string, string> cache = new Dictionary<string, string>();

        public static Activity Load(string path, string fallbackPath)
        {
            string activityDefinition = null;

            if (!cache.ContainsKey(path))
            {
                string definitionPath = File.Exists(path) ? path : fallbackPath;

                if (!File.Exists(definitionPath))
                    throw new FileNotFoundException(String.Format("Could not load activity definition from either {0} or {1}.", path, fallbackPath));

                using (StreamReader sr = new StreamReader(definitionPath))
                {
                    activityDefinition = sr.ReadToEnd();
                }

                cache[path] = activityDefinition;
            }
            
            Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(activityDefinition));
            Activity wf = ActivityXamlServices.Load(stream, new ActivityXamlServicesSettings { CompileExpressions = true });            
            return wf;
        }
    }
}
