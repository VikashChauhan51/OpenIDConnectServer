using Duende.IdentityServer.Models;
using Duende.IdentityServer.AspNetIdentity;
using IdentityServer.IDP.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.IDP.Services;

public class UserProfileService : ProfileService<ApplicationUser>
{
    public UserProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        : base(userManager, claimsFactory)
    {
    }

    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
    {
        var principal = await GetUserClaimsAsync(user);
        //var id = (ClaimsIdentity)principal.Identity;   
        context.AddRequestedClaims(principal.Claims);
    }
}
