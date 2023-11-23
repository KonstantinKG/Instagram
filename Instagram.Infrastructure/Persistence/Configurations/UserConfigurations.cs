using Instagram.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .UseSerialColumn();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(32);
        builder.Property(u => u.Fullname)
            .HasColumnName("fullname");
        builder.Property(u => u.Phone)
            .HasColumnName("phone")
            .HasMaxLength(15);
        builder.Property(u => u.Email)
            .HasColumnName("email");
        builder.Property(u => u.Password)
            .HasColumnName("password");
    }
}