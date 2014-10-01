using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation.ServiceModel;

namespace VHA.ServiceFoundation.Data
{
    public interface IAddUpdateNotificationSink
    {
        void OnAddUpdateSuccess(Message request);
    }
}
