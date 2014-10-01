using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml;

namespace VHA.ServiceFoundation.Security
{
    /// <summary>
    /// Simple ClaimsAuthorizationManager implementation that reads policy information from the .config file
    /// </summary>
    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        private static Dictionary<ResourceAction, Func<ClaimsPrincipal, bool>> _policies =
            new Dictionary<ResourceAction, Func<ClaimsPrincipal, bool>>();

        private PolicyReader _policyReader = new PolicyReader();

        /// <summary>
        /// Creates a new instance of the MyClaimsAuthorizationManager
        /// </summary>        
        public CustomClaimsAuthorizationManager()
        {
        }

        /// <summary>
        /// Overloads  the base class method to load the custom policies from the config file
        /// </summary>
        /// <param name="nodelist">XmlNodeList containing the policy information read from the config file</param>
        public override void LoadCustomConfiguration(XmlNodeList nodelist)
        {
            Expression<Func<ClaimsPrincipal, bool>> policyExpression;

            foreach (XmlNode node in nodelist)
            {
                //
                // Initialize the policy cache
                //
                XmlDictionaryReader rdr = XmlDictionaryReader.CreateDictionaryReader(
                    new XmlTextReader(new StringReader(node.OuterXml)));
                rdr.MoveToContent();

                string resource = rdr.GetAttribute("resource");
                string action = rdr.GetAttribute("action");

                policyExpression = _policyReader.ReadPolicy(rdr);

                //
                // Compile the policy expression into a function
                //
                Func<ClaimsPrincipal, bool> policy = policyExpression.Compile();

                //
                // Insert the policy function into the policy cache
                //
                _policies[new ResourceAction(resource, action)] = policy;
            }
        }

        /// <summary>
        /// Checks if the principal specified in the authorization context is authorized to perform specified action
        /// on the specified resoure
        /// </summary>
        /// <param name="pec">Authorization context</param>
        /// <returns>true if authorized, false otherwise</returns>
        public override bool CheckAccess(AuthorizationContext pec)
        {
            //
            // Evaluate the policy against the claims of the 
            // principal to determine access
            //
            bool access = false;
            try
            {

                var resource = (pec.Resource.First<Claim>().Value.Contains('/'))
                                   ? new Uri(pec.Resource.First<Claim>().Value)
                                         .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped)
                                   : pec.Resource.First<Claim>().Value;

                var ra = new ResourceAction(resource, pec.Action.First<Claim>().Value);

                access = _policies[ra](pec.Principal);
            }
            catch (Exception)
            {
                // if no policy specified for the URI, allow access
                // if a custom policy is specified then make sure that the policy is satisfied.
                return pec.Resource.First<Claim>().Value.Contains('/');
                //access = false;
            }

            return access;
        }

        public static bool CheckAccess(string resource, string action)
        {
            var context = new AuthorizationContext(
                ClaimsPrincipal.Current,
                resource,
                action);

            return new ClaimsAuthorizationManager().CheckAccess(context);
        }

        public static Dictionary<string, string> TransalateActiveDirectorySids(List<string> sids)
        {
            var toReturn = new Dictionary<string, string>();

            var groups = new IdentityReferenceCollection();

            sids.ForEach(s => groups.Add(new SecurityIdentifier(s)));

            var names = groups.Translate(typeof(NTAccount), false);

            for (var i = 0; i < sids.Count; i++)
                toReturn.Add(sids[i], names[i].Value);

            return toReturn;
        }

        public static IList<ClaimData> GetCurrentUserClaims()
        {
            var toReturn = new List<ClaimData>();

            var sids = ClaimsPrincipal.Current.Claims.Where(
                s => s.Type == ClaimTypes.GroupSid)
                                      .Select(x => x.Value)
                                      .ToList();

            var groupNames = CustomClaimsAuthorizationManager.TransalateActiveDirectorySids(sids.ToList());

            foreach (var claim in ClaimsPrincipal.Current.Claims)
            {
                var val = (claim.Type == ClaimTypes.GroupSid)
                              ? claim.Value + " [" + groupNames[claim.Value] + "]"
                              : claim.Value;

                toReturn.Add(new ClaimData { Type = claim.Type, Value = val });

            }

            return toReturn;
        }
    }
}
