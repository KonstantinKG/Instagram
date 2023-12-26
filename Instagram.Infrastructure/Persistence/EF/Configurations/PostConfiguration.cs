using Microsoft.EntityFrameworkCore;

using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.TagAggregate.ValueObjects;
using Instagram.Domain.Aggregates.UserAggregate;


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

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            
            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasMaxLength(2200);

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

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("now()");

            builder.HasMany(x => x.PostLikes)
                .WithOne()
                .HasForeignKey(x => x.PostId);
            
            builder.HasMany(x => x.Galleries)
                .WithOne()
                .HasForeignKey("post_id")
                .IsRequired();
            
            builder.HasMany(x => x.Comments)
                .WithOne()
                .HasForeignKey("post_id")
                .IsRequired();

            builder.HasMany(x => x.Tags)
                .WithMany()
                .UsingEntity<PostToTag>(
                    "posts_to_tags",
                    left => left.HasOne<Tag>().WithMany().HasForeignKey(p => p.TagId).OnDelete(DeleteBehavior.Cascade),
                    right => right.HasOne<Post>().WithMany().HasForeignKey(t => t.PostId).OnDelete(DeleteBehavior.Cascade),
                    eb =>
                    {
                            eb.Property(p => p.PostId)
                                .HasColumnName("post_id");

                            eb.Property(p => p.TagId)
                                .HasColumnName("tag_id");
                            
                            eb.HasKey(x => new {x.PostId, x.TagId});
                        }
                    );
            
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
    
    private static void ConfigurePostGalleryTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostGallery>(builder =>
        {
            builder.ToTable("posts_gallery");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

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
                .HasForeignKey(x => x.PostId);
            
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
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
            
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Comments)
                .WithOne()
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
                .HasForeignKey(x => x.CommentId);
            
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        });
    }
}