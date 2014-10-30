using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    public class ComplexFilter
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string Operator { get; set; }
    }
}