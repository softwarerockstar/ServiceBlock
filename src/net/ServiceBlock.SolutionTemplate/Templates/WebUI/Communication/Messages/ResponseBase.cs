using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Communication
{
    public abstract class ResponseBase
    {   
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}