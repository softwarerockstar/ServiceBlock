using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBlock.Foundation.Data;

namespace ServiceBlock.Foundation.Activities
{
    [Designer("System.Activities.Core.Presentation.SequenceDesigner, System.Activities.Core.Presentation")]
    [DisplayName("Data Context")]
    [ExcludeFromCodeCoverage]
    public class DataContextActivity : NativeActivity
    {
        [RequiredArgument]
        public InArgument<IUnitOfWork> UnitOfWork { get; set; }

        // Child activities collection
        private Collection<Activity> _activities;
        private Collection<Variable> _variables;

        // Pointer to the current item in the collection being executed
        Variable<int> _currentItem = new Variable<int>() { Default = 0 };

        public DataContextActivity()
        {
            _activities = new Collection<Activity>();
            _variables = new Collection<Variable>();
        }

        // Collection of children to be executed sequentially
        [Browsable(false)]
        public Collection<Activity> Activities
        {
            get { return _activities; }
        }

        // Collection of variables
        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get { return _variables; }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.SetChildrenCollection(_activities);
            metadata.SetVariablesCollection(_variables);
            metadata.AddImplementationVariable(_currentItem);
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.Properties.Add("DataContextActivity.UnitOfWork", UnitOfWork.Get(context));

            //Schedule the first activity
            if (this.Activities.Count > 0)
                context.ScheduleActivity(this.Activities[0], this.OnChildCompleted);
        }

        void OnChildCompleted(NativeActivityContext context, ActivityInstance completed)
        {
            // Calculate the index of the next activity to scheduled            
            int next = _currentItem.Get(context) + 1;

            // If index within boundaries...
            if (next < this.Activities.Count)
            {
                // Schedule the next activity
                context.ScheduleActivity(this.Activities[next], this.OnChildCompleted);

                // Store the index in the collection of the activity executing
                this._currentItem.Set(context, next);
            }
        }
    }
}
