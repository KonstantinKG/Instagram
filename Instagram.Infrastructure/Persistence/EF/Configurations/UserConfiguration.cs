﻿using Microsoft.EntityFrameworkCore;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public static class UserConfiguration
{
    public static void ConfigureUser(this ModelBuilder modelBuilder)
    {
        ConfigureUserTable(modelBuilder);
        ConfigureGenderTable(modelBuilder);
        ConfigureProfileTable(modelBuilder);
        ConfigureSubscriptionsTable(modelBuilder);
    }

    private static void ConfigureUserTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

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
            
            builder.HasOne(x => x.Profile)
                .WithOne()
                .HasForeignKey<UserProfile>(x => x.UserId)
                .IsRequired();
        });
    }
    
    private static void ConfigureProfileTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(builder =>
        {
            builder.ToTable("users_profiles");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(p => p.UserId)
                .HasColumnName("user_id");
            
            builder.Property(p => p.Bio)
                .HasColumnName("bio")
                .HasMaxLength(150);

            builder.Property(p => p.Image)
                .HasColumnName("image");
            
            builder.Property(p => p.GenderId)
                .HasColumnName("gender_id");

            builder.HasOne(x => x.Gender)
                .WithMany()
                .HasForeignKey(x => x.GenderId)
                .IsRequired(false);
        });
    }

    private static void ConfigureGenderTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGender>(builder =>
        {
            builder.ToTable("users_genders");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .UseSerialColumn();
            
            builder.Property(x => x.Name)
                .HasColumnName("name");
            
            builder.HasIndex(x => x.Name).IsUnique();
        });
    }
    
    private static void ConfigureSubscriptionsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSubscription>(builder =>
        {
            builder.ToTable("users_subscriptions");
            
            builder.HasKey(x => new {x.SubscriberId, x.UserId});

            builder.Property(x => x.SubscriberId)
                .HasColumnName("subscriber_id")
                .ValueGeneratedNever();
            
            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedNever();
                        
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}