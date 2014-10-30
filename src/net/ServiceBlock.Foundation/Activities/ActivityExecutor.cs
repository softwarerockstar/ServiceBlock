using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity;
using DigitalMediaStore.EnterpriseFramework.Data;
using DigitalMediaStore.EnterpriseFramework.ExceptionManagement;
using System.ServiceModel;

namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    [ExcludeFromCodeCoverage]
    public class ActivityExecutor<T, K> where T : class where K : class
    {
        private readonly Activity _activity;
        private readonly ValidatorFactory _validatorFactory;
        private readonly IUnitOfWork _databaseContext;

        public ActivityExecutor(
            IUnitOfWork unitOfWork, 
            ValidatorFactory validatorFactory, 
            Activity activity)
        {
            _activity = activity;
            _validatorFactory = validatorFactory;
            _databaseContext = unitOfWork;
        }

        public K Execute(T request, string validationRuleSetName = null)
        {
            var fault = ValidateRequest(request, validationRuleSetName);

            if (fault != null)
                throw new FaultException<ServiceValidationFault>(fault, new FaultReason("Validation failed"));

            Dictionary<string, object> arguments = new Dictionary<string, object>
            {
                { "request", request },                
                { "unitOfWork", _databaseContext }
            };
            
            var results = WorkflowInvoker.Invoke(_activity, arguments);

            return results["response"] as K;
        }

        private ServiceValidationFault ValidateRequest(
            T request, 
            string validationRuleSetName)
        {
            validationRuleSetName = (validationRuleSetName == null) ? 
                String.Empty : validationRuleSetName;
            
            var validator = _validatorFactory.CreateValidator<T>(validationRuleSetName);

            var validationResults = validator.Validate(request);

            if (!validationResults.IsValid)
            {
                var fault = new ServiceValidationFault();

                foreach (var item in validationResults)
                    fault.Details.Add(item.Message);

                return fault;
            }

            return null;
        }
    }
}


