using IdentityServer.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.DbContexts;

public class IdentityDbContext : DbContext
{

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<UserClaim> UserClaims { get; set; } = null!;

    public DbSet<UserLogin> UserLogins { get; set; } = null!;

    public DbSet<UserSecret> UserSecrets { get; set; } = null!;


    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .OfType<IConcurrencyAware>();

        foreach (var entry in updatedConcurrencyAwareEntries)
        {
            entry.ConcurrencyStamp = Guid.NewGuid().ToString();
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
