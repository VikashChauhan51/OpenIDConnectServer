using IdentityServer.IDP.Entities;
using System.Security.Claims;

namespace IdentityServer.IDP.Services
{
    public interface ILocalUserService
    {
        Task<bool> ActivateUserAsync(string securityCode);
        Task AddExternalProviderToUser(string subject, string provider, string providerIdentityKey);
        void AddUser(User userToAdd, string password);
        Task<bool> AddUserSecret(string subject, string name, string secret);
        User AutoProvisionUser(string provider, string providerIdentityKey, IEnumerable<Claim> claims);
        Task<User> FindUserByExternalProviderAsync(string provider, string providerIdentityKey);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserBySubjectAsync(string subject);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject);
        Task<UserSecret> GetUserSecretAsync(string subject, string name);
        Task<bool> IsUserActive(string subject);
        Task<bool> SaveChangesAsync();
        Task<bool> ValidateCredentialsAsync(string userName, string password);
    }
}