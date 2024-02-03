using Microsoft.EntityFrameworkCore;

using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;


namespace Instagram.Infrastructure.Persistence.EF.Configurations;

public static class PostConfiguration
{
    public static void ConfigurePost(this ModelBuilder modelBuilder)
    {
        ConfigurePostTable(modelBuilder);
        ConfigurePostLikeTable(modelBuilder);
        ConfigurePostGalleryTable(modelBuilder);
        ConfigurePostCommentTable(modelBuilder);
        ConfigurePostCommentLikeTable(modelBuilder);
        ConfigurePostToTagsTable(modelBuilder);
    }
    
    private static void ConfigurePostTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(builder =>
        {
            builder.ToTable("posts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();
            
            builder.Property(x => x.UserId)
                .HasColumnName("user_id");
            
            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasMaxLength(2200)
                .IsRequired(false);

            builder.Property(x => x.LocationId)
                .HasColumnName("location_id");

            builder
                .HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired(false);
            
            builder.Property(x => x.Views)
                .HasColumnName("views");

            builder.Property(x => x.HideStats)
                .HasColumnName("hide_stats");

            builder.Property(x => x.HideComments)
                .HasColumnName("hide_comments");

            builder.Property(x => x.Active)
                .HasColumnName("active");
            
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.Ignore(x => x.CommentsCount);
            builder.Ignore(x => x.LikesCount);

            builder.HasMany(x => x.PostLikes)
                .WithOne()
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(x => x.Galleries)
                .WithOne()
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            builder.HasMany(x => x.Comments)
                .WithOne()
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(x => x.Tags)
                .WithMany()
                .UsingEntity<PostToTag>();
            
            builder.Metadata.FindNavigation(nameof(Post.Galleries))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Metadata.FindNavigation(nameof(Post.Comments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Metadata.FindNavigation(nameof(Post.PostLikes))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Metadata.FindNavigation(nameof(Post.Tags))?
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        });
    }
    
    private static void ConfigurePostToTagsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostToTag>(builder =>
        {
            builder.ToTable("posts_to_tags");
            
            builder.HasKey(x => new {x.PostId, x.TagId});

            builder.Property(p => p.PostId)
                .HasColumnName("post_id");
                        
            builder.Property(p => p.TagId)
                .HasColumnName("tag_id");

            builder.HasOne<Tag>()
                .WithMany()
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Post>()
                .WithMany()
                .HasForeignKey(t => t.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigurePostGalleryTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostGallery>(builder =>
        {
            builder.ToTable("posts_gallery");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.PostId)
                .HasColumnName("post_id");

            builder.Property(x => x.Description)
                .HasColumnName("description");

            builder.Property(x => x.File)
                .HasColumnName("file");

            builder.Property(x => x.Labels)
                .HasColumnName("labels");

        });
    }
    
    private static void ConfigurePostLikeTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostLike>(builder =>
        {
            builder.ToTable("posts_likes");

            builder.HasKey(x => new {x.PostId, x.UserId});

            builder.Property(x => x.PostId)
                .HasColumnName("post_id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.HasOne<Post>()
                .WithMany(x => x.PostLikes)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.PostId);
        });
    }
    
    private static void ConfigurePostCommentTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostComment>(builder =>
        {
            builder.ToTable("posts_comments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Content)
                .HasColumnName("content");

            builder.Property(x => x.ParentId)
                .HasColumnName("parent_id");
            
            builder.Property(x => x.UserId)
                .HasColumnName("user_id");
            
            builder.Property(x => x.PostId)
                .HasColumnName("post_id");
            
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.HasMany(x => x.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.ParentId);

            builder.HasMany(p => p.CommentLikes);
            
            builder.Metadata.FindNavigation(nameof(PostComment.CommentLikes))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Metadata.FindNavigation(nameof(PostComment.Comments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

        });
    }

    private static void ConfigurePostCommentLikeTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostCommentLike>(builder =>
        {
            builder.ToTable("posts_comments_likes");
            
            builder.HasKey(x => new {x.CommentId, x.UserId});

            builder.Property(x => x.CommentId)
                .HasColumnName("comment_id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.HasOne<PostComment>()
                .WithMany(x => x.CommentLikes)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.CommentId);
        });
    }
}