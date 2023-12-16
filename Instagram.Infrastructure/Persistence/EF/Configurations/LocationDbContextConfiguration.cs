using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public class LocationDbContextConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        ConfigureLocationTable(builder);
    }

    private void ConfigureLocationTable(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => LocationId.Fill(value)
            );

        builder.Property(x => x.Name)
            .HasColumnName("name");
    }
}