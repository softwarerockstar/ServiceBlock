using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.ExceptionManagement
{
    [ExcludeFromCodeCoverage]
    public class FriendlyErrorMessage
    {
        public string DeclaringTypeName { get; set; }
        public string CallingMethodName { get; set; }
        public string ExceptionTypeName { get; set; }
        public string FriendlyMessage { get; set; }
    }
}
