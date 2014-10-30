using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using RestSharp;
using Newtonsoft.Json;

namespace WebUI.Communication
{
    #region IServiceClient
    public partial interface IServiceClient
    {
        TResponse Add<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
        TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
        TResponse GetAll<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
        TResponse GetById<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
        TResponse GetWithCriteria<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
        TResponse Update<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new();
    }
    #endregion

    #region RestServiceClient
    public partial class RestServiceClient : IServiceClient
    {
        private readonly RestClient _restClient;

        public RestServiceClient(string baseUri)
        {
            _restClient = new RestClient(baseUri.Trim().TrimEnd('/'));
        }

        public TResponse Add<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {

            var body = JsonConvert.SerializeObject(request);

            var restRequest = new RestRequest(Method.PUT)
                .AddParameter("text/json", body, ParameterType.RequestBody);

            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            dynamic dynRequest = request;
            var id = dynRequest.Id;

            var restRequest = new RestRequest(String.Format("/{0}", id), Method.DELETE);

            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse GetAll<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            var restRequest = new RestRequest(Method.GET);

            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = JsonConvert.DeserializeObject<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse GetById<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            dynamic dynRequest = request;
            var id = dynRequest.Id;

            var restRequest = new RestRequest(String.Format("/{0}", id), Method.GET);

            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse GetWithCriteria<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            dynamic dynRequest = request;
            dynamic criteria = dynRequest.Criteria;

            var props = ((object)criteria).GetType()
                                           .GetProperties()
                                           .Where(x => x.GetValue(criteria) != null)
                                           .Select(
                                               p => SerializeCriteriaProperty(criteria, p));
            
            var url = "/ByCriteria/?" + String.Join("&", props.ToArray());

            var restRequest = new RestRequest(url, Method.GET);
            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse Update<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            var body = JsonConvert.SerializeObject(request);
            
            var restRequest = new RestRequest(Method.POST)
                .AddParameter("text/json", body, ParameterType.RequestBody);

            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        private void EnsureSuccessResponse(IRestResponse response)
        {
            if (new HttpStatusCode[] { HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest }
                .Contains(response.StatusCode))
            {
                var newException = new InvalidOperationException(response.StatusDescription);
                newException.Data["DetailHtml"] = response.Content;
                throw newException;
            }
        }

        private string SerializeCriteriaProperty(object criteria, PropertyInfo p)
        {
            if (new string[] { "ComplexFilters", "AdditionalInfo" }.Contains(p.Name))
                return p.Name + "=" + HttpUtility.UrlEncode(JsonConvert.SerializeObject(p.GetValue(criteria)));
            else
                return p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(criteria).ToString());
        }

    }

    #endregion
}