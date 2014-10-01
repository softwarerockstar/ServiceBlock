using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Runtime.Caching;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using VHA.ServiceFoundation.Composition;

namespace VHA.ServiceFoundation.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        private readonly MemoryCache _cache = new MemoryCache("ClaimsCache");

        [Import(typeof(ICustomClaimsEnricher))]
        public ICustomClaimsEnricher CustomClaimsEnricher { get; set; }

        public ClaimsTransformer()
        {
            try
            {
                CompositionManager.Container.ComposeParts(this);
            }
            catch (Exception)
            {
                // No custom claims manager provided
                //TODO: Log
            }
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {

            return (incomingPrincipal.Identity.IsAuthenticated) ?
                TransformClaims(resourceName, incomingPrincipal) :
                incomingPrincipal;
        }

        private ClaimsPrincipal TransformClaims(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            int cacheSlidingExpirationSeconds = 10;

            if (_cache[incomingPrincipal.Identity.Name] == null)
            {
                if (CustomClaimsEnricher != null)
                {
                    cacheSlidingExpirationSeconds = CustomClaimsEnricher.CacheSlidingExpirationSeconds;
                    var newClaims = CustomClaimsEnricher.GetAdditionalClaims(resourceName, incomingPrincipal);

                    foreach (var claim in newClaims)
                        ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(claim);
                }

                var p = new CacheItemPolicy();
                p.SlidingExpiration = TimeSpan.FromSeconds(cacheSlidingExpirationSeconds);

                _cache.Add(incomingPrincipal.Identity.Name, incomingPrincipal, p);
            }

            return _cache[incomingPrincipal.Identity.Name] as ClaimsPrincipal;
        }
    }
}
