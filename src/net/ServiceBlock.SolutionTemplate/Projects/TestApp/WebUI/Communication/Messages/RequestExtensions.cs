using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public static class RequestExtensions
    {
        public static dynamic GetPostData(this HttpRequestBase request)
        {
            var props = new ExpandoObject();

            foreach (var key in request.Form.AllKeys)
                ((IDictionary<string, object>)props)[key] = request.Form[key];

            dynamic toReturn = props;

            return toReturn;
        }

    }
}