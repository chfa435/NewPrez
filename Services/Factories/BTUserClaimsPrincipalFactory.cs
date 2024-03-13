using System.Security.Claims;
using NewTiceAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace NewTiceAI.Services.Factories
{
    public class BTUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<TAUser, IdentityRole>
    {
        public BTUserClaimsPrincipalFactory(UserManager<TAUser> userManager,
                                            RoleManager<IdentityRole> roleManager,
                                            IOptions<IdentityOptions> optionsAccessor)
         : base(userManager, roleManager, optionsAccessor)
        {
        }

        //Custom claim
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TAUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("OrganizationId", user.OrganizationId.ToString()));
            return identity;    
        }

        //MFA 2FA claim
        public async override Task<ClaimsPrincipal> CreateAsync(TAUser user)
        {
            var principal = await base.CreateAsync(user);
            ClaimsIdentity identity = (ClaimsIdentity)principal.Identity!;

            var claims = new List<Claim>();

            if (user.TwoFactorEnabled)
            {
                claims.Add(new Claim("amr", "mfa"));
            }
            else
            {
                claims.Add(new Claim("amr", "pwd"));
            }

            identity.AddClaims(claims);
            return principal;
        }

    }
}
