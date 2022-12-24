

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.Entities;

public class UserLogin: IConcurrencyAware
{
    public Guid Id { get; set; }
    public string Provider { get; set; }
    public string ProviderIdentityKey { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.Property(secret => secret.Id).IsRequired();
        builder.Property(secret => secret.Provider).IsRequired().HasMaxLength(200);
        builder.Property(secret => secret.ProviderIdentityKey).IsRequired().HasMaxLength(200);
        builder.Property(secret => secret.ConcurrencyStamp).IsRequired().HasMaxLength(50).IsConcurrencyToken();
        builder.Property(secret => secret.UserId).IsRequired();
        builder.HasKey(secret => secret.Id);
        builder.Navigation(secret => secret.User);
        builder.ToTable("UserLogins");
    }
}