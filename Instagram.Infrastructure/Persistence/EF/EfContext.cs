using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Infrastructure.Persistence.EF.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.EF;

public class EfContext : DbContext
{
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    
    public EfContext(DbContextOptions<EfContext> options) 
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureUserProfile();
        builder.ConfigurePost();
        builder.ConfigureTag();
        builder.ConfigureLocation();
        
        base.OnModelCreating(builder);
    }
}