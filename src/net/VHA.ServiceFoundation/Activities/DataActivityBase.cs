using System;
using System.Activities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Activities
{
    [ExcludeFromCodeCoverage]
    public abstract class DataActivityBase: NativeActivity
    {
        protected Action<NativeActivityContext> CustomExecuteActivity { get; set; }

        protected abstract void ExecuteActivity(NativeActivityContext context);

        protected DataActivityBase()
        {
            Constraints.Add(
                ActivityConstraints.MustBeInsideOuterActivityConstraint<DataContextActivity, DataActivityBase>("This activity can only be placed inside a UnitOfWork activity."));
        }

        protected T GetUnitOfWork<T>(NativeActivityContext context) where T : class
        {
            return context.Properties.Find("DataContextActivity.UnitOfWork") as T;
        }

        protected sealed override void Execute(NativeActivityContext context)
        {
            if (this.CustomExecuteActivity != null)
                this.CustomExecuteActivity(context);
            else
                this.ExecuteActivity(context);
        }

        
    }

    [ExcludeFromCodeCoverage]
    public abstract class DataActivityBase<T> : NativeActivity<T>
    {
        protected Action<NativeActivityContext> CustomExecuteActivity { get; set; }

        protected abstract void ExecuteActivity(NativeActivityContext context);

        protected DataActivityBase()
        {
            Constraints.Add(
                ActivityConstraints.MustBeInsideOuterActivityConstraint<DataContextActivity, DataActivityBase<T>>("This activity can only be placed inside a UnitOfWork activity."));
        }

        protected K GetUnitOfWork<K>(NativeActivityContext context) where K : class
        {
            return context.Properties.Find("DataContextActivity.UnitOfWork") as K;
        }

        protected sealed override void Execute(NativeActivityContext context)
        {
            if (this.CustomExecuteActivity != null)
                this.CustomExecuteActivity(context);
            else
                this.ExecuteActivity(context);
        }
    }
    

}
