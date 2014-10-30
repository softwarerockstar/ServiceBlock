using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Communication
{
    public class GetWithCriteriaRequest : RequestBase
    {
        public Criteria Criteria { get; set; }
    }
}