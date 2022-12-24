using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? Password { get; set; }
    public bool Active { get; set; }
    public string Email { get; set; } = default!;
    public string? EmailSecurityCode { get; set; }
    public DateTime? EmailSecurityCodeExpirationDate { get; set; }
    public string Phone { get; set; } = default!;
    public string? PhoneSecurityCode { get; set; }
    public DateTime? PhoneSecurityCodeExpirationDate { get; set; }
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
    public ICollection<UserSecret> Secrets { get; set; } = new List<UserSecret>();
}


public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Id).IsRequired();
        builder.Property(user => user.Subject).IsRequired().HasMaxLength(50);
        builder.Property(user => user.UserName).IsRequired().HasMaxLength(50);
        builder.Property(user => user.ConcurrencyStamp).IsRequired().HasMaxLength(50).IsConcurrencyToken();
        builder.Property(user => user.Email).IsRequired().HasMaxLength(50);
        builder.Property(user => user.Phone).IsRequired().HasMaxLength(15);
        builder.Property(user => user.Password).HasMaxLength(200);
        builder.Property(user => user.EmailSecurityCode).HasMaxLength(200);
        builder.Property(user => user.PhoneSecurityCode).HasMaxLength(200);
        builder.Property(user => user.Active);
        builder.Property(user => user.EmailSecurityCodeExpirationDate);
        builder.Property(user => user.PhoneSecurityCodeExpirationDate);
        builder.HasKey(user => user.Id);
        builder.Navigation(user => user.Claims);
        builder.Navigation(user => user.Logins);
        builder.Navigation(user => user.Secrets);
        builder.ToTable("Users");
    }
}