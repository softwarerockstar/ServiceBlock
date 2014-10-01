using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VHA.ServiceFoundation.ExceptionManagement;

namespace VHA.ServiceFoundation
{
    public static class ExceptionExtensions
    {
        public static void SetHttpError(this FaultException<BusinessServiceFault> ex)
        {
            if (WebOperationContext.Current != null)
            {
                var message = ex.Detail.Html;
                var response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(message);
            }
        }

        public static void SetHttpError(this FaultException<ServiceValidationFault> ex)
        {
            if (WebOperationContext.Current != null)
            {
                var message = ex.Detail.Html;
                var response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusDescription = ex.Message;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(message);

            }
        }

        public static void SetHttpError(this Exception ex)
        {   
            if (WebOperationContext.Current != null)
            {
                string message = String.Format(
                    "<br/>ErrorID - {0}", 
                    Trace.CorrelationManager.ActivityId);
                
                if (HttpContext.Current.Request["Debug"] == "true")
                    message = message + "<br/>" + ex.ToString();

                var response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.StatusDescription = "Oops! Your last operation failed.";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(message);
            }
        }

    }
}
