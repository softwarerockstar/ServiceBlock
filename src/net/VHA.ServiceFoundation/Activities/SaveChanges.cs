using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHA.ServiceFoundation.Data;

namespace VHA.ServiceFoundation.Activities
{
    [ExcludeFromCodeCoverage]
    public class SaveChanges : DataActivityBase
    {
        public OutArgument<int> RowCount { get; set; }

        protected override void ExecuteActivity(NativeActivityContext context)
        {
            var dataContext = base.GetUnitOfWork<IUnitOfWork>(context);
            var result = dataContext.SaveChanges();
            RowCount.Set(context, result);
        }
    }
}
