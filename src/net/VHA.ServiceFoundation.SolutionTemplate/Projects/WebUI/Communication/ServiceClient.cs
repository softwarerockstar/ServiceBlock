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

    #region SoapServiceClient
    public partial class SoapServiceClient<TClientType> : IServiceClient
where TClientType : class
    {
        private ClientBase<TClientType> _repository;

        public SoapServiceClient(ClientBase<TClientType> client)
        {
            _repository = client;
        }

        public TResponse Add<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("AddOrUpdate", new object[] { request });
        }

        public TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("Delete", new object[] { request });
        }

        public TResponse GetAll<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("GetAll", new object[] { request });
        }

        public TResponse GetById<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("GetById", new object[] { request });
        }

        public TResponse GetWithCriteria<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("GetWithCriteria", new object[] { request });
        }

        public TResponse Update<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            return InvokeDynamicMethod<TResponse>("AddOrUpdate", new object[] { request });
        }

        private T InvokeDynamicMethod<T>(string methodName, object[] args) where T : class
        {
            try
            {
                var toReturn = _repository.GetType().InvokeMember(
                    methodName,
                    BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    _repository,
                    args) as T;

                return toReturn;
            }
            catch (Exception ex)
            {
#if DEBUG
                string message = (ex.InnerException != null) ?
                    ex.InnerException.Message :
                    "Unknown error occured";

                var newException = new InvalidOperationException(message, ex.InnerException);
#else
                var newException = new InvalidOperationException("An error occured calling the service.");
#endif
                newException.Data["DetailHtml"] = GetPropertyValue<string>(ex.InnerException, "Detail.Html");
                throw newException;
            }
        }

        T GetPropertyValue<T>(object obj, string propertyName) where T : class
        {
            if (obj == null) return null;

            foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
            {
                if (prop == null) return null;
                obj = prop.GetValue(obj, null);
            }

            return obj as T;
        }
    }
    #endregion

    #region RestServiceClient
    public partial class RestServiceClient<TClientType> : IServiceClient
where TClientType : class
    {
        private readonly RestClient _restClient;
        private static Dictionary<string, ChannelEndpointElement> _endpointContracts;

        static RestServiceClient()
        {
            _endpointContracts = new Dictionary<string, ChannelEndpointElement>();
            var clientSection = (ClientSection)WebConfigurationManager.GetSection("system.serviceModel/client");
            foreach (ChannelEndpointElement endpoint in clientSection.Endpoints)
                _endpointContracts[endpoint.Contract] = endpoint;
        }

        public RestServiceClient()
        {
            var items = _endpointContracts.Where(s => s.Key.EndsWith(typeof(TClientType).Name));

            if (items.Count() == 0)
                throw new InvalidOperationException("No endpoint defined for " + typeof(TClientType).Name);

            var endpoint = items.First().Value;

            var svcPath = endpoint.Address.AbsolutePath;

            string baseUri = String.Format("{0}://{1}/rest{2}",
                                endpoint.Address.Scheme,
                                endpoint.Address.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped),
                                svcPath.Remove(svcPath.LastIndexOf("Service.svc")));

            _restClient = new RestClient(baseUri);

        }

        public RestServiceClient(string baseUri)
        {
            _restClient = new RestClient(baseUri.Trim().TrimEnd('/'));
        }

        public TResponse Add<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            var entities = request.GetType().GetProperty("Entities").GetValue(request);

            var body = new JavaScriptSerializer().Serialize(new { AdditionalInfo = (object)null, Entities = entities });

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
            var idList = request.GetType().GetProperty("IdList").GetValue(request);

            var body = new JavaScriptSerializer().Serialize(new { AdditionalInfo = (object)null, IdList = idList });

            var restRequest = new RestRequest(Method.DELETE)
                .AddParameter("text/json", body, ParameterType.RequestBody);

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

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        public TResponse GetById<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            var id = request.GetType().GetProperty("Id").GetValue(request);
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
            dynamic criteria = request.GetType().GetProperty("Criteria").GetValue(request);

            var props = ((object)criteria).GetType()
                                           .GetProperties()
                                           .Where(x => x.GetValue(criteria) != null)
                                           .Select(
                                               p => SerializeCriteriaProperty(criteria, p));

            var url = "/?criteria&" + String.Join("&", props.ToArray());

            var restRequest = new RestRequest(url, Method.GET);
            var response = _restClient.Execute(restRequest);
            EnsureSuccessResponse(response);

            var toReturn = new JavaScriptSerializer().Deserialize<TResponse>(response.Content);
            return toReturn;
        }

        private string SerializeCriteriaProperty(object criteria, PropertyInfo p)
        {
            if (p.Name == "ComplexFilters")
                return p.Name + "=" + HttpUtility.UrlEncode(new JavaScriptSerializer().Serialize(p.GetValue(criteria)));
            else
                return p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(criteria).ToString());
        }

        public TResponse Update<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class, new()
        {
            var entities = request.GetType().GetProperty("Entities").GetValue(request);

            var body = new JavaScriptSerializer().Serialize(new { AdditionalInfo = (object)null, Entities = entities });

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
    }

    #endregion
}