using Instagram.Domain.Aggregates.TagAggregate;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public static class TagConfiguration
{
    public static void ConfigureTag(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(builder =>
        {
            builder.ToTable("tags");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.HasIndex(x => x.Name).IsUnique();
        });
    }
}