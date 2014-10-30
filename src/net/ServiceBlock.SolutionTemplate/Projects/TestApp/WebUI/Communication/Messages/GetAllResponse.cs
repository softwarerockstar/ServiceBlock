using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public class GetAllResponse : ResponseBase
    {
        public IList<object> Result { get; set; }
    }
}