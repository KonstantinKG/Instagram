using Microsoft.EntityFrameworkCore;
using Instagram.Domain.Aggregates.TokenAggregate;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public static class JwtConfiguration
{
    public static void ConfigureJwt(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Token>(builder =>
        {
            builder.ToTable("tokens");

            builder.HasKey(x => x.Id);
        
            builder.HasIndex(x => new { x.UserId, x.SessionId }).IsUnique();

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .UseSerialColumn();
        
            builder.Property(x => x.SessionId)
                .HasColumnName("session_id");
        
            builder.Property(x => x.UserId)
                .HasColumnName("user_id");
        
            builder.Property(x => x.Hash)
                .HasColumnName("hash");
        });
    }
}