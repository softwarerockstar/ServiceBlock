using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceBlock.Foundation.ExceptionManagement
{
    //TODO: Test this!!
    [ExcludeFromCodeCoverage]
    public static class FriendlyErrorMessageMapper
    {
        private static Dictionary<string, List<FriendlyErrorMessage>> _cache;

        private static void EnsureFriendlyMessageCacheCreated(string cultureName)
        {
            if (_cache == null)
                _cache = new Dictionary<string, List<FriendlyErrorMessage>>();

            if (!_cache.ContainsKey(cultureName))
            {
                _cache.Add(cultureName, new List<FriendlyErrorMessage>());

                var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("FriendlyErrorMessages.{0}.config", cultureName));

                if (File.Exists(configFilePath))
                {
                    var doc = XDocument.Load(configFilePath);

                    if (doc != null)
                    {
                        try
                        {
                            _cache[cultureName].AddRange(doc.Root.Elements()
                                .Where(x => x.Name == "errorMessage")
                                .Select(x => new FriendlyErrorMessage
                                {
                                    DeclaringTypeName = x.Attribute("declaringTypeName").Value,
                                    CallingMethodName = x.Attribute("callingMethodName").Value,
                                    ExceptionTypeName = x.Attribute("exceptionTypeName").Value,
                                    FriendlyMessage = x.Attribute("friendlyMessage").Value

                                }).ToList());
                        }
                        finally
                        {
                            // Config file is not in expected format so it won't be used.                            
                        }
                    }
                }
            }
        }

        public static string GetFriendlyErrorMessage(this Exception exception, object source, [CallerMemberName] string callingMethodName = "")
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;
            EnsureFriendlyMessageCacheCreated(cultureName);

            var declaringTypeName = source.GetType().FullName;
            var exceptionTypeName = exception.GetType().FullName;

            var matchingItems = _cache[cultureName].Where(
                x => x.DeclaringTypeName == declaringTypeName &&
                    x.CallingMethodName == callingMethodName);

            FriendlyErrorMessage matchingItem = null;
            
            matchingItem = matchingItems.Where(x => x.ExceptionTypeName == exceptionTypeName).FirstOrDefault();

            if (matchingItem == null)
                matchingItem = matchingItems.Where(x => x.ExceptionTypeName == "*").FirstOrDefault();

            return (matchingItem != null) ? matchingItem.FriendlyMessage : null;
        }
    }
}
