using Microsoft.AspNetCore.Identity;

namespace IdentityServer.IDP.Entities;

public class ApplicationUser : IdentityUser, IConcurrencyAware
{
}
