using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.TagAggregate.ValueObjects;

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
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TagId.Fill(value)
                );

            builder.Property(x => x.Name)
                .HasColumnName("name");
        });
    }
}