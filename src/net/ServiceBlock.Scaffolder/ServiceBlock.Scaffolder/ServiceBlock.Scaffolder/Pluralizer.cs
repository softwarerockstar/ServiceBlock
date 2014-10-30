using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder
{
    public static class Pluralizer
    {
        private static PluralizationService _pluralizationService;

        static Pluralizer()
        {
            _pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
        }

        public static string Pluralize(string word)
        {
            return _pluralizationService.Pluralize(word ?? String.Empty);
        }

        public static string Singularize(string word)
        {
            return _pluralizationService.Singularize(word ?? String.Empty);
        }

        public static bool IsSingular(string word)
        {
            return _pluralizationService.IsSingular(word ?? String.Empty);
        }

        public static bool IsPlural(string word)
        {
            return _pluralizationService.IsPlural(word ?? String.Empty);
        }
    }
}
