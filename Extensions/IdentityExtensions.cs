using System.Security.Claims;
using System.Security.Principal;

namespace NewTiceAI.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetOrganizationId(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("OrganizationId")!;
            return int.Parse(claim.Value);
        }

        //public static int GetAccountId(this IIdentity identity)
        //{
        //    Claim claim = ((ClaimsIdentity)identity).FindFirst("AccountId")!;
        //    return int.Parse(claim.Value);
        //}
    }
}
