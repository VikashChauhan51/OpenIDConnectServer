using IdentityServer.IDP.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.IDP.DbContexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        base.ChangeTracker.LazyLoadingEnabled = false;
        base.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       // modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);

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
