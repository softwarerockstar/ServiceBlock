using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation
{
    public static class StringExtensions
    {
        public static string EmptyToDefault(this string s, string defaultValue)
        {
            return (String.IsNullOrWhiteSpace(s) ? defaultValue : s);
        }

        public static bool IsNumeric(this string value)
        {
            return (value.EmptyToDefault("a").
                ToCharArray().Where(x => !Char.IsDigit(x)).Count() == 0);
        }
    }
}
