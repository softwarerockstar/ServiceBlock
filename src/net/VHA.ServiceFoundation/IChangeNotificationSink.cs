using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation
{
    public interface IChangeNotificationSink
    {
        void OnChange(string activityTypeName, object request, object response);
    }
}
