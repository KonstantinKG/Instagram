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

    /* REMOVE IN THE FUTURE WHEN SEPARATE AUTH SERVER */
    public DbSet<Token> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var configuration = new UserDbContextConfiguration();
        builder.ApplyConfiguration(configuration);
        
        var jwtTokenTempConfiguration = new JwtTokenTempConfigurations();
        builder.ApplyConfiguration(jwtTokenTempConfiguration);
        
        base.OnModelCreating(builder);
    }
}