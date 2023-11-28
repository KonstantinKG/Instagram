using Instagram.Domain.Aggregates.TokenAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public class JwtTokenTempConfigurations : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        ConfigureTokensTable(builder);
    }

    private void ConfigureTokensTable(EntityTypeBuilder<Token> builder)
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
    }
}