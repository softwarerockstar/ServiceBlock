 using System;
using System.Diagnostics;

namespace VHA.ServiceFoundation.Diagnostics
{
    public class ActivityTracerScope : IDisposable
    {
        private readonly Guid oldActivityId;
        private readonly Guid newActivityId;
        private readonly TraceSource ts;
        private readonly string activityName;

        public ActivityTracerScope(TraceSource ts, string activityName)
        {
            this.ts = ts;
            this.oldActivityId = Trace.CorrelationManager.ActivityId;
            this.activityName = activityName;

            this.newActivityId = Guid.NewGuid();

            if (this.oldActivityId != Guid.Empty)
            {
                ts.TraceTransfer(0, "Transferring to new activity...", newActivityId);
            }
            Trace.CorrelationManager.ActivityId = newActivityId;
            ts.TraceEvent(TraceEventType.Start, 0, activityName);
        }
        public void Dispose()
        {
            if (this.oldActivityId != Guid.Empty)
            {
                ts.TraceTransfer(0, "Transferring back to old activity...", oldActivityId);
            }
            ts.TraceEvent(TraceEventType.Stop, 0, activityName);
            Trace.CorrelationManager.ActivityId = oldActivityId;
        }
    }
}