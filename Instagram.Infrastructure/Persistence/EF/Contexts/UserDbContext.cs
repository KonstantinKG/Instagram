using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Infrastructure.Persistence.EF.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.EF.Contexts;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var configuration = new UserDbContextConfiguration();
        builder.ApplyConfiguration(configuration);
        base.OnModelCreating(builder);
    }
}