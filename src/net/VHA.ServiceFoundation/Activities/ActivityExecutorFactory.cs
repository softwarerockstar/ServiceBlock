using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;


namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    [ExcludeFromCodeCoverage]
    public static class ActivityExecutorFactory
    {
        private readonly static IUnityContainer _container;
        private readonly static ActivityExecutionInfo _activityExecutionInfo;

        static ActivityExecutorFactory()
        {
            string containerName = ConfigurationManager.AppSettings["DefaultUnityContainerName"] ?? "defaultContainer";

            if (containerName == null)
                throw new ApplicationException("Either a Unity container named defaultContainer must be present or DefaultUnityContainerName setting must be specified in AppSettings");

            _container = new UnityContainer()
                .LoadConfiguration(containerName);

            List<string> _alreadyRegistered = new List<string>();

            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();

            _activityExecutionInfo = _container.Resolve<ActivityExecutionInfo>();

            foreach (var executionOverride in _activityExecutionInfo.ActivityExecutionOverrides)
            {
                string name = String.Format("{0}.{1}", executionOverride.EntityName, executionOverride.OperationName);

                _container.RegisterType(
                    typeof(Activity), 
                    executionOverride.WorkflowType, 
                    name);
                
                _alreadyRegistered.Add(name);
            }

            var defaultAssembly= Assembly.Load(_activityExecutionInfo.DefaultWorkflowAssembly);

            var defaultWorkflowTypes = defaultAssembly.GetTypes()
                .Where(x => x.FullName.Contains(String.Format("{0}.", _activityExecutionInfo.DefaultWorkflowNamespace)) && !x.FullName.Contains("+"));

            foreach (var workflowType in defaultWorkflowTypes)
            {                
                var name = workflowType.FullName.Remove(0, _activityExecutionInfo.DefaultWorkflowNamespace.Length + 1);

                if (!_alreadyRegistered.Contains(name))
                    _container.RegisterType(
                        typeof(Activity), 
                        workflowType,
                        name);
            }
        }


        public static ActivityExecutor<T, K> CreateActivityExecutor<T, K>(string entityName, string operationName)
            where T : class
            where K : class
        {
            var activity = _container.Resolve<Activity>(String.Format("{0}.{1}",
                entityName,
                operationName));

            return _container.Resolve<ActivityExecutor<T, K>>(
                new ParameterOverride("activity", activity));

            _activityExecutionInfo
        }

    }
}
