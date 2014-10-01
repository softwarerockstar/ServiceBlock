using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;


namespace VHA.ServiceFoundation.Orchestrations
{
    [ExcludeFromCodeCoverage]
    public static class OrchestrationExecutorFactory
    {
        private readonly static IUnityContainer _container;
        private readonly static OrchestrationExecutionInfo _orchestrationExecutionInfo;

        static OrchestrationExecutorFactory()
        {
            string containerName = ConfigurationManager.AppSettings["DefaultUnityContainerName"] ?? "defaultContainer";

            if (containerName == null)
                throw new ApplicationException("Either a Unity container named defaultContainer must be present or DefaultUnityContainerName setting must be specified in AppSettings");

            _container = new UnityContainer()
                .LoadConfiguration(containerName);

            List<string> _alreadyRegistered = new List<string>();

            _container.AddNewExtension<EnterpriseLibraryCoreExtension>();

            _orchestrationExecutionInfo = _container.Resolve<OrchestrationExecutionInfo>();

            foreach (var executionOverride in _orchestrationExecutionInfo.ExecutionOverrides)
            {
                string name = String.Format("{0}.{1}", executionOverride.EntityName, executionOverride.OperationName);

                _container.RegisterType(
                    typeof(Activity), 
                    executionOverride.WorkflowType, 
                    name);
                
                _alreadyRegistered.Add(name);
            }

            var defaultAssembly= Assembly.Load(_orchestrationExecutionInfo.DefaultWorkflowAssembly);

            var defaultWorkflowTypes = defaultAssembly.GetTypes()
                .Where(x => x.FullName.Contains(String.Format("{0}.", _orchestrationExecutionInfo.DefaultWorkflowNamespace)) && !x.FullName.Contains("+"));

            foreach (var workflowType in defaultWorkflowTypes)
            {                
                var name = workflowType.FullName.Remove(0, _orchestrationExecutionInfo.DefaultWorkflowNamespace.Length + 1);

                if (!_alreadyRegistered.Contains(name))
                    _container.RegisterType(
                        typeof(Activity), 
                        workflowType,
                        name);
            }
        }


        public static OrchestrationExecutor<T, K> CreateExecutor<T, K>(string entityName, string operationName, string validationRulesetName = null)
            where T : class
            where K : class
        {
            var activity = _container.Resolve<Activity>(String.Format("{0}.{1}",
                entityName,
                operationName));

            var uow = _container.Resolve<IUnitOfWork>();
            var validationFactory = _container.Resolve<ValidatorFactory>();
            var changeNotificationSink = _container.IsRegistered<IChangeNotificationSink>() ?       
                _container.Resolve<IChangeNotificationSink>() : null;

            return new OrchestrationExecutor<T, K>(
                uow,
                validationFactory,
                changeNotificationSink,
                activity,  
                validationRulesetName,
                _orchestrationExecutionInfo.NullRequestCheckEnabled);
        }

    }
}
