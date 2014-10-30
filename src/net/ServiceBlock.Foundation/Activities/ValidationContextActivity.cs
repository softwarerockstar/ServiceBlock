using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalMediaStore.EnterpriseFramework.Data;

namespace DigitalMediaStore.EnterpriseFramework.Activities
{
    [Designer("System.Activities.Core.Presentation.SequenceDesigner, System.Activities.Core.Presentation")]
    [DisplayName("Data Context")]
    [ExcludeFromCodeCoverage]
    public class ValidationContextActivity : NativeActivity
    {
        private Sequence _innerSequence;

        public ValidationContextActivity()
        {
            _innerSequence = new Sequence();
        }

        [Browsable(false)]
        public Collection<Activity> Activities
        {
            get
            {
                return _innerSequence.Activities;
            }
        }

        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                return _innerSequence.Variables;
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddImplementationChild(_innerSequence);
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleActivity(_innerSequence);
        }

    }
}
