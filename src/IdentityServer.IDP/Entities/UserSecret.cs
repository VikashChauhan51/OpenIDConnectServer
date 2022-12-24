using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.Entities;
public class UserSecret
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Secret { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}

public class UserSecretConfiguration : IEntityTypeConfiguration<UserSecret>
{
    public void Configure(EntityTypeBuilder<UserSecret> builder)
    {
        builder.Property(secret => secret.Id).IsRequired();
        builder.Property(secret => secret.Name).IsRequired().HasMaxLength(50);
        builder.Property(secret => secret.Secret).IsRequired().HasMaxLength(500);
        builder.Property(secret => secret.ConcurrencyStamp).IsRequired().HasMaxLength(50).IsConcurrencyToken();
        builder.Property(secret => secret.UserId).IsRequired();
        builder.HasKey(secret => secret.Id);
        builder.Navigation(secret => secret.User);
        builder.ToTable("UserSecrets");
    }
}
