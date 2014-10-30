using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public class DeleteRequest : RequestBase
    {
        public int Id { get; set; }
    }
}