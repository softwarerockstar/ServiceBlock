using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Foundation.Security
{
    public interface ICustomClaimsEnricher
    {
        int CacheSlidingExpirationSeconds { get; }

        IEnumerable<Claim> GetAdditionalClaims(string resourceName, ClaimsPrincipal incomingPrincipal);
    }
}
