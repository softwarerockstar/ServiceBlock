using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity;
using ServiceBlock.Foundation.Data;
using ServiceBlock.Foundation.ExceptionManagement;
using System.ServiceModel;

namespace ServiceBlock.Foundation.Orchestrations
{
    [ExcludeFromCodeCoverage]
    public class OrchestrationExecutor<T, K> where T : class where K : class
    {
        public Activity Workflow { get; private set; }
        public ValidatorFactory ValidatorFactory { get; private set; }
        public IChangeNotificationSink ChangeNotificationSink { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }
        public string ValidationRuleSetName { get; private set; }
        public bool NullRequestCheckEnabled { get; private set; }


        public OrchestrationExecutor(
            IUnitOfWork unitOfWork, 
            ValidatorFactory validatorFactory,
            IChangeNotificationSink changeNotificationSink,
            Activity activity,
            string validationRuleSetName,
            bool nullRequestCheckEnabled)
        {
            this.Workflow = activity;
            this.ValidatorFactory = validatorFactory;
            this.ChangeNotificationSink = changeNotificationSink;
            this.UnitOfWork = unitOfWork;
            this.ValidationRuleSetName = validationRuleSetName;
            this.NullRequestCheckEnabled = nullRequestCheckEnabled;
        }

        public K Execute(T request, bool raiseChangeEvent = false)
        {
            var fault = ValidateRequest(request);

            if (fault != null)
                throw new FaultException<ServiceValidationFault>(fault, new FaultReason("Validation failed"));

            Dictionary<string, object> arguments = new Dictionary<string, object>
            {
                { "request", request },                
                { "unitOfWork", this.UnitOfWork }
            };
            
            var results = WorkflowInvoker.Invoke(this.Workflow, arguments);
            var response = results["response"] as K;

            if (this.ChangeNotificationSink != null && raiseChangeEvent)
                Task.Factory.StartNew(() =>
                    this.ChangeNotificationSink.OnChange(
                        this.Workflow.GetType().FullName,
                        request,
                        response));

            return response;
        }


        private ServiceValidationFault ValidateRequest(T request)
        {
            var fault = new ServiceValidationFault { Message = "Request validation failed." };

            if (this.NullRequestCheckEnabled && request == null)
            {
                fault.Details.Add("Request cannot be null");
                return fault;
            }

            var validationRuleSetName = (this.ValidationRuleSetName == null) ?
                String.Empty : this.ValidationRuleSetName;

            var validator = this.ValidatorFactory.CreateValidator<T>(validationRuleSetName);

            var validationResults = validator.Validate(request);

            if (!validationResults.IsValid)
            {
                foreach (var item in validationResults)
                    fault.Details.Add(item.Message);

                return fault;
            }

            return null;
        }
    }
}


