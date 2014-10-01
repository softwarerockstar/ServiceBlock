using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation
{
    public interface IChangeNotificationSink
    {
        void OnChange(string activityTypeName, object request, object response);
    }
}
