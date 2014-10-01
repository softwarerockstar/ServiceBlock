using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation
{
    public static class DateTimeUtil
    {
        public static int ToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            return Int32.Parse(dateTime.ToString(format));
        }

        public static DateTime ToDate(int intDate, string format = "yyyyMMdd")
        {
            var outDate = DateTime.MinValue;
            DateTime.TryParseExact(intDate.ToString(), format, CultureInfo.InvariantCulture,
                                   DateTimeStyles.None, out outDate);

            return outDate;
        }

    }
}
