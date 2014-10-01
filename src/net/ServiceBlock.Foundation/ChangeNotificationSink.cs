using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation
{
    public class ChangeNotificationSink : IChangeNotificationSink
    {
        public void OnChange(string activityTypeName, object request, object response)
        {
            Debug.WriteLine(request.ToString());
            Debug.WriteLine(response.ToString());
        }
    }
}
