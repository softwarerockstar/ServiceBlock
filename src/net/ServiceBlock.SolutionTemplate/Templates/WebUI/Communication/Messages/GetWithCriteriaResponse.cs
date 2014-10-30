using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Communication
{
    public class Result
    {
        public IList<dynamic> Body { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetWithCriteriaResponse :ResponseBase
    {
        public Result Result { get; set; }
    }
}