using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using ServiceBlock.Foundation;
using ServiceBlock.Foundation.ExceptionManagement;
using ServiceBlock.Foundation.Orchestrations;

namespace ServiceBlock.Foundation.ServiceModel.Web
{
    public static class OrchestrationExecutorExtensions
    {
        public static K ExecuteRest<T, K>(this OrchestrationExecutor<T, K> executor, T request, bool raiseChangeEvent = false)
            where T : class
            where K : class
        {
            try
            {
                return executor.Execute(request, raiseChangeEvent);
            }
            catch (FaultException<BusinessServiceFault> ex)
            {
                ex.SetHttpError();
                return null;
            }
            catch (FaultException<ServiceValidationFault> ex)
            {
                ex.SetHttpError();
                return null;
            }
            catch (Exception ex)
            {
                ex.SetHttpError();
                return null;
            }
        }

    }
}
