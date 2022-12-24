

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.Entities;

public class UserClaim
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.Property(claim => claim.Id).IsRequired();
        builder.Property(claim => claim.Type).IsRequired().HasMaxLength(200);
        builder.Property(claim => claim.Value).IsRequired().HasMaxLength(500);
        builder.Property(claim => claim.ConcurrencyStamp).IsRequired().HasMaxLength(50).IsConcurrencyToken();
        builder.Property(claim => claim.UserId).IsRequired();
        builder.HasKey(claim => claim.Id);
        builder.Navigation(claim => claim.User);
        builder.ToTable("UserClaims");
    }
}