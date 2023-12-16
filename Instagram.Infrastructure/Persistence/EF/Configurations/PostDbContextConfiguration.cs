using Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public class PostDbContextConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        ConfigurePostTable(builder);
        ConfigurePostLikesTable(builder);
        ConfigurePostGalleryTable(builder);
        ConfigurePostCommentTable(builder);
        ConfigurePostTagTable(builder);
    }

    private void ConfigurePostTable(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => PostId.Fill(value)
            );


        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => UserId.Fill(value)
            );

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        
        
        builder.Property(x => x.Content)
            .HasColumnName("content")
            .HasMaxLength(2200);

        builder.Property(x => x.LocationId)
            .HasColumnName("location_id")
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                value => value == null ? null : LocationId.Fill((Guid)value)
            );

        builder
            .HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId);


        builder.Property(x => x.Views)
            .HasColumnName("views");

        builder.Property(x => x.HideStats)
            .HasColumnName("hide_stats");

        builder.Property(x => x.HideComments)
            .HasColumnName("hide_comments");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("created_at")
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("now()");
    }

    private void ConfigurePostGalleryTable(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(x => x.Galleries, gb =>
        {
            gb.ToTable("posts_gallery");

            gb.WithOwner().HasForeignKey("post_id");

            gb.HasKey(x => x.Id);

            gb.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => PostGalleryId.Fill(value)
                );

            gb.Property(x => x.Description)
                .HasColumnName("description");

            gb.Property(x => x.File)
                .HasColumnName("file");

            gb.Property(x => x.Labels)
                .HasColumnName("labels");

        });
        
        builder.Metadata.FindNavigation(nameof(Post.Galleries))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigurePostCommentTable(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(x => x.Comments, gb =>
        {
            gb.ToTable("posts_comments");

            gb.WithOwner().HasForeignKey("post_id");

            gb.HasKey(x => x.Id);

            gb.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => PostCommentId.Fill(value)
                );

            gb.Property(x => x.Content)
                .HasColumnName("content");

            gb.Property(x => x.ParentId)
                .HasColumnName("parent_id")
                .HasConversion(
                    id => id == null ? (Guid?)null : id.Value,
                    value => value == null ? null : PostCommentId.Fill((Guid)value)
                );
            
            gb.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasConversion(
                    id => id.Value,
                    value => UserId.Fill(value)
                );

            gb
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            gb.OwnsMany(x => x.UsersLikedIds, ub =>
            {
                ub.ToTable("posts_comments_likes");

                ub.WithOwner().HasForeignKey("comment_id");
                
                ub.Property<long>("id")
                    .UseSerialColumn();
                
                ub.HasKey("id");

                ub.Property(x => x.Value)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();
            });
            
            gb.Property(x => x.CreatedAt)
                .HasColumnName("created_ad")
                .HasDefaultValueSql("now()");

        });
        
        builder.Metadata.FindNavigation(nameof(Post.Comments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Metadata.FindNavigation(nameof(PostComment.UsersLikedIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Metadata.FindNavigation(nameof(PostComment.Comments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigurePostLikesTable(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(x => x.UsersLikedIds, ub =>
        {
            ub.ToTable("posts_likes");

            ub.WithOwner().HasForeignKey("post_id");
            
            ub.Property<long>("id")
                .UseSerialColumn();
            
            ub.HasKey("id");
            
            ub.Property(x => x.Value)
                .HasColumnName("user_id")
                .ValueGeneratedNever();
            
        });
        
        builder.Metadata.FindNavigation(nameof(Post.UsersLikedIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigurePostTagTable(EntityTypeBuilder<Post> builder)
    {
        builder
            .HasMany(x => x.Tags)
            .WithMany()
            .UsingEntity(eb =>
            {
                eb.ToTable("posts_tags");
                eb.HasOne(typeof(Post)).WithMany().HasForeignKey("post_id").HasPrincipalKey(nameof(Post.Id));
                eb.HasOne(typeof(Tag)).WithMany().HasForeignKey("tag_id").HasPrincipalKey(nameof(Tag.Id));
                
                eb.HasKey("post_id", "tag_id");

            });
        
        builder.Metadata.FindNavigation(nameof(Post.Tags))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}