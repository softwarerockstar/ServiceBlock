using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Security
{
    public interface ICustomClaimsEnricher
    {
        int CacheSlidingExpirationSeconds { get; }

        IEnumerable<Claim> GetAdditionalClaims(string resourceName, ClaimsPrincipal incomingPrincipal);
    }
}
