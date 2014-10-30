using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public class AddOrUpdateResponse : ResponseBase
    {
        public IList<dynamic> Result { get; set; }

        public int RowCount { get; set; }
    }
}