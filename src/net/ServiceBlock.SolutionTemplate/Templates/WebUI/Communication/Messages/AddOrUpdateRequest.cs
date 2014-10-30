using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Communication
{
    public class AddOrUpdateRequest : RequestBase
    {
        public IList<dynamic> Entities { get; set; }
    }
}