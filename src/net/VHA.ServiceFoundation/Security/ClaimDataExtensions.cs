using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;

namespace ServiceBlock.Foundation.Security
{
    public static class ClaimDataExtensions
    {
        public static bool Exists(this IList<ClaimData> claims, string claimType, string claimValue)
        {
            return claims.Where(x => x.Type == claimType && x.Value == claimValue)
                         .Count() > 0;
        }

        public static bool Assert(this IList<ClaimData> claims, string claimType, string claimValue)
        {
            if (!Exists(claims, claimType, claimValue))
                throw new SecurityAccessDeniedException("Access denied.");

            return true;
        }

        public static void Assert(this IList<ClaimData> claims, Func<IList<ClaimData>, bool> func)
        {
            if (!func(claims))
                throw new SecurityAccessDeniedException("Access denied.");
        }

    }
}
