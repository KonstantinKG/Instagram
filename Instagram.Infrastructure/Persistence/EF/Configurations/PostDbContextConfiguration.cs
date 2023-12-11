using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public class PostDbContextConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        ConfigurePostTable(builder);
    }

    private void ConfigurePostTable(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .UseSerialColumn();


        builder.Property(x => x.UserId)
            .HasColumnName("user_id");
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id);
        
        
        builder.Property(x => x.Content)
            .HasColumnName("content")
            .HasMaxLength(2200);
        
        builder.Property(x => x.LocationId)
            .HasColumnName("location_id");
        
        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .HasPrincipalKey(x => x.Id);
        
        
        builder.Property(x => x.Views)
            .HasColumnName("views");
        
        builder.Property(x => x.HideStats)
            .HasColumnName("hide_stats");
        
        builder.Property(x => x.HideComments)
            .HasColumnName("hide_comments");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}