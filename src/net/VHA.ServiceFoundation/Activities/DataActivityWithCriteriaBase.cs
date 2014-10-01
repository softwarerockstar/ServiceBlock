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
    public abstract class DataActivityWithCriteriaBase<T> : DataActivityBase<PagingList<T>>
    {
        [RequiredArgument]
        public InArgument<Criteria> Criteria { get; set; }

        public InArgument<string> IncludeProperties { get; set; }

        protected IQueryable<T> Query { get; set; }

        //protected Action<NativeActivityContext> CustomExecuteActivity { get; set; }

        //protected abstract void ExecuteActivity(NativeActivityContext context);

        //protected sealed override void Execute(NativeActivityContext context)
        //{
        //    if (this.CustomExecuteActivity != null)
        //        this.CustomExecuteActivity(context);
        //    else
        //        this.ExecuteActivity(context);
        //}
    }
}
