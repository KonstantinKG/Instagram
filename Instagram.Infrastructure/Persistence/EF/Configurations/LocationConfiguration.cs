using Microsoft.EntityFrameworkCore;
using Instagram.Domain.Aggregates.LocationAggregate;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public static class LocationConfiguration
{
    public static void ConfigureLocation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(builder =>
        {
            builder.ToTable("locations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .UseSerialColumn();

            builder.Property(x => x.Name)
                .HasColumnName("name");
        });
    }
}