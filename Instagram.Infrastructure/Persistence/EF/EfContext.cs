using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.TokenAggregate;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Infrastructure.Persistence.EF.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.EF;

public class EfContext : DbContext
{
    public EfContext(DbContextOptions<EfContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    /* REMOVE IN THE FUTURE WHEN SEPARATE AUTH SERVER */
    public DbSet<Token> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var configuration = new UserDbContextConfiguration();
        builder.ApplyConfiguration(configuration);
        
        var jwtTokenTempConfiguration = new JwtTokenTempConfigurations();
        builder.ApplyConfiguration(jwtTokenTempConfiguration);

        var postDbContextConfiguration = new PostDbContextConfiguration();
        builder.ApplyConfiguration(postDbContextConfiguration);
        
        var locationDbContextConfiguration = new LocationDbContextConfiguration();
        builder.ApplyConfiguration(locationDbContextConfiguration);
        
        var tagDbContextConfiguration = new TagDbContextConfiguration();
        builder.ApplyConfiguration(tagDbContextConfiguration);
        
        base.OnModelCreating(builder);
    }
}