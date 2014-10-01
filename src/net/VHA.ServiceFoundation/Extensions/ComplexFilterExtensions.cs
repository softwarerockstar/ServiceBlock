using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation
{
    public static class ComplexFilterExtensions
    {
        public static void WhenExists(
            this List<ComplexFilter> complexFilters, 
            string fieldName, 
            Action<string> fieldValueAction)
        {
            var filter = complexFilters.FirstOrDefault(s => s.FieldName == fieldName);
            if (filter != null)
            {
                var filterValue = filter.FieldValue;
                fieldValueAction(filterValue);
            }

        }

    }
}
