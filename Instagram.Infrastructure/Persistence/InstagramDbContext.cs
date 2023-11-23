using Instagram.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence;

public class InstagramDbContext : DbContext
{
    public InstagramDbContext(DbContextOptions<InstagramDbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(InstagramDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}