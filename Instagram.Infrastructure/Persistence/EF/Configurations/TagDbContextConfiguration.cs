using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.TagAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public class TagDbContextConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        ConfigureTagTable(builder);
    }

    private void ConfigureTagTable(EntityTypeBuilder<Tag> builder)
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
    }
}