using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.Dapper.Repositories;

public class DapperPostRepository : IDapperPostRepository
{
    private readonly DapperContext _context;

    public DapperPostRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetPost(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
            SELECT 
                p.id, 
                p.user_id as UserId, 
                p.content, 
                l.id as LocationId, 
                p.views, 
                p.hide_stats as HideStats, 
                p.hide_comments as HideComments, 
                p.active as Active,
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            WHERE p.id = @id
            ";

        var post = await connection.QueryFirstOrDefaultAsync<Post>(sql, parameters);
        return post;
    }
    
    public async Task<Post?> GetPostWithGalleries(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
            SELECT 
                p.id, 
                p.user_id as UserId, 
                p.content, 
                l.id as LocationId, 
                p.views, 
                p.hide_stats as HideStats, 
                p.hide_comments as HideComments, 
                p.active as Active,
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt,
                pg.id as Id,
                pg.description as Description,
                pg.file as File,
                pg.labels as Labels
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            LEFT JOIN public.posts_gallery pg on p.id = pg.post_id
            WHERE p.id = @id
            ";

        Post? postEntry = null;
        var gallerySet = new HashSet<Guid>();
        var post = await connection.QueryAsync<Post, PostGallery, Post>(
            sql,
            (post, gallery) =>
            {
                if (postEntry == null)
                    postEntry = post;

                if (gallery == null || gallery.Id == Guid.Empty)
                    return postEntry;

                if (!gallerySet.Contains(gallery.Id))
                {
                    post.AddGallery(gallery);
                    gallerySet.Add(gallery.Id);
                }

                return postEntry;
            },
            parameters,
            splitOn: "Id");
        
        return post.DistinctBy(x => x.Id).FirstOrDefault();
    }
    
    public async Task<Post?> GetFullPost(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            """
                SELECT DISTINCT 
                    p.id,
                    p.user_id as UserId,
                    p.content as Content,
                    l.id as LocationId,
                    p.views as Views,
                    p.hide_stats as HideStats,
                    p.hide_comments as HideComments,
                    p.created_at as CreatedAt,
                    p.updated_at as UpdatedAt,
                    pg.id as Id,
                    pg.file as File,
                    pg.description as Description,
                    pg.labels as Labels,
                    u.id as Id,
                    u.username as Username,
                    up.fullname as Fullname,
                    up.id as Id,
                    up.Image as Image,
                    t.id as Id,
                    t.name as Name,
                    pc.id as Id,
                    pl.post_id as PostId,
                    pl.user_id as UserId
                FROM posts p
                INNER JOIN posts_gallery pg ON p.id = pg.post_id
                INNER JOIN public.users u on u.id = p.user_id
                INNER JOIN public.users_profiles up on u.id = up.user_id
                LEFT JOIN locations l ON p.location_id = l.id
                LEFT JOIN public.posts_to_tags ptt on p.id = ptt.post_id
                LEFT JOIN public.tags t on t.id = ptt.tag_id
                LEFT JOIN public.posts_comments pc on p.id = pc.post_id
                LEFT JOIN public.posts_likes pl on p.id = pl.post_id
                WHERE p.id = @id and p.active = true;
            """;

        var postDictionary = new Dictionary<Guid, Post>();
        var postGalleries = new HashSet<Guid>();
        var postTags = new HashSet<Guid>();
        var postComments = new HashSet<Guid>();
        var postLikes = new HashSet<string>();
        
        var posts = await connection.QueryAsync<Post, PostGallery, User, UserProfile, Tag, PostComment, PostLike, Post>(
            sql,
            (post, gallery, user, profile, tag, comment, like) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out Post? postEntry))
                {
                    postEntry = post;
                    postDictionary.Add(postEntry.Id, postEntry);
                }
                
                postEntry.User = user;
                user.Profile = profile;

                if (!postGalleries.Contains(gallery.Id))
                {
                    postEntry.AddGallery(gallery);
                    postGalleries.Add(gallery.Id);
                }

                if (tag?.Name != null && !postTags.Contains(tag.Id))
                {
                    postEntry.AddTag(tag);
                    postTags.Add(tag.Id);
                }

                if (comment != null && comment.Id != Guid.Empty && !postComments.Contains(comment.Id))
                {
                    postEntry.CommentsCount += 1;
                    postComments.Add(comment.Id);
                }

                if (like != null && like.PostId != Guid.Empty && like.UserId != Guid.Empty && !postLikes.Contains($"{like.UserId}{like.PostId}"))
                {
                    postEntry.LikesCount += 1;
                    postLikes.Add($"{like.UserId}{like.PostId}");
                }                    
                
                return postEntry;
            },
            parameters,
            splitOn: "Id,Id,Id,Id,Id,PostId"
        );
        
        return posts.DistinctBy(x => x.Id).FirstOrDefault();
    }

    public async Task<Tag?> GetTag(string name)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Name = name };
        const string sql = "SELECT t.id, t.name FROM tags t WHERE t.name = @name;";
        var tag = await connection.QueryFirstOrDefaultAsync<Tag>(sql, parameters);
        return tag;
    }

    public async Task<List<Tag>> GetPostTags(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
                SELECT t.id, t.name FROM posts_to_tags pt
                INNER JOIN public.tags t on t.id = pt.tag_id
                WHERE pt.post_id = @id ;
            ";

        var tags = await connection.QueryAsync<Tag>(sql, parameters);
        return tags.ToList();
    }

    public async Task<List<Post>> AllPosts(int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit, Date = date };
        const string sql = 
            """
                SELECT DISTINCT 
                    p.id,
                    p.user_id as UserId,
                    p.content as Content,
                    l.id as LocationId,
                    p.views as Views,
                    p.hide_stats as HideStats,
                    p.hide_comments as HideComments,
                    p.created_at as CreatedAt,
                    p.updated_at as UpdatedAt,
                    pg.id as Id,
                    pg.file as File,
                    pg.description as Description,
                    pg.labels as Labels,
                    u.id as Id,
                    u.username as Username,
                    up.fullname as Fullname,
                    up.id as Id,
                    up.Image as Image,
                    t.id as Id,
                    t.name as Name,
                    pc.id as Id,
                    pl.post_id as PostId,
                    pl.user_id as UserId
                FROM posts p
                INNER JOIN posts_gallery pg ON p.id = pg.post_id
                INNER JOIN public.users u on u.id = p.user_id
                INNER JOIN public.users_profiles up on u.id = up.user_id
                LEFT JOIN locations l ON p.location_id = l.id
                LEFT JOIN public.posts_to_tags ptt on p.id = ptt.post_id
                LEFT JOIN public.tags t on t.id = ptt.tag_id
                LEFT JOIN public.posts_comments pc on p.id = pc.post_id
                LEFT JOIN public.posts_likes pl on p.id = pl.post_id
                ORDER BY p.created_at DESC
                LIMIT @limit
                OFFSET @offset
            """;

        var postDictionary = new Dictionary<Guid, Post>();
        var postGalleries = new HashSet<Guid>();
        var postTags = new HashSet<Guid>();
        var postComments = new HashSet<Guid>();
        var postLikes = new HashSet<string>();
        
        var posts = await connection.QueryAsync<Post, PostGallery, User, UserProfile, Tag, PostComment, PostLike, Post>(
            sql,
            (post, gallery, user, profile, tag, comment, like) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out Post? postEntry))
                {
                    postEntry = post;
                    postDictionary.Add(postEntry.Id, postEntry);
                }
                
                postEntry.User = user;
                user.Profile = profile;

                if (!postGalleries.Contains(gallery.Id))
                {
                    postEntry.AddGallery(gallery);
                    postGalleries.Add(gallery.Id);
                }

                if (tag?.Name != null && !postTags.Contains(tag.Id))
                {
                    postEntry.AddTag(tag);
                    postTags.Add(tag.Id);
                }

                if (comment != null && comment.Id != Guid.Empty && !postComments.Contains(comment.Id))
                {
                    postEntry.CommentsCount += 1;
                    postComments.Add(comment.Id);
                }

                if (like != null && like.PostId != Guid.Empty && like.UserId != Guid.Empty && !postLikes.Contains($"{like.UserId}{like.PostId}"))
                {
                    postEntry.LikesCount += 1;
                    postLikes.Add($"{like.UserId}{like.PostId}");
                }                    
                
                return postEntry;
            },
            parameters,
            splitOn: "Id,Id,Id,Id,Id,PostId"
        );
        
        return posts.DistinctBy(x => x.Id).ToList();
    }
    public async Task<List<Post>> AllUserPosts(Guid userId, int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId, Offset = offset, Limit = limit, Date = date };
        const string sql = 
            """
            
                        SELECT
                            p.id,
                            p.user_id as UserId,
                            p.content as Content,
                            l.id as LocationId,
                            p.views as Views,
                            p.hide_stats as HideStats,
                            p.hide_comments as HideComments,
                            p.created_at as CreatedAt,
                            p.updated_at as UpdatedAt,
                            pg.id as Id,
                            pg.file as File,
                            pg.description as Description,
                            pg.labels as Labels,
                            pc.id as Id,
                            pl.post_id as PostId,
                            pl.user_id as UserId
                        FROM posts p
                        INNER JOIN posts_gallery pg ON p.id = pg.post_id
                        LEFT JOIN locations l ON p.location_id = l.id
                        LEFT JOIN public.posts_comments pc on p.id = pc.post_id
                        LEFT JOIN public.posts_likes pl on p.id = pl.post_id
                        WHERE
                            p.user_id = @userId and
                            p.created_at < @date and
                            p.active = true
                        ORDER BY p.created_at DESC
                        LIMIT @limit
                        OFFSET @offset
                        
            """;

        var postDictionary = new Dictionary<Guid, Post>();
        var postGalleries = new HashSet<Guid>();
        var postComments = new HashSet<Guid>();
        var postLikes = new HashSet<string>();
        
        var posts = await connection.QueryAsync<Post, PostGallery, PostComment, PostLike, Post>(
            sql,
            (post, gallery, comment, like) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out Post? postEntry))
                {
                    postEntry = post;
                    postDictionary.Add(postEntry.Id, postEntry);
                }

                if (!postGalleries.Contains(gallery.Id))
                {
                    postEntry.AddGallery(gallery);
                    postGalleries.Add(gallery.Id);
                }

                if (comment != null && comment.Id != Guid.Empty && !postComments.Contains(comment.Id))
                {
                    postEntry.CommentsCount += 1;
                    postComments.Add(comment.Id);
                }

                if (like != null && like.PostId != Guid.Empty && like.UserId != Guid.Empty && !postLikes.Contains($"{like.UserId}{like.PostId}"))
                {
                    postEntry.LikesCount += 1;
                    postLikes.Add($"{like.UserId}{like.PostId}");
                }                    
                
                return postEntry;
            },
            parameters,
            splitOn: "Id,Id,PostId"
        );
        
        return posts.DistinctBy(x => x.Id).ToList();
    }
    public async Task<List<Post>> AllHomePosts(Guid subscriberId, int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { SubscriberId = subscriberId, Offset = offset, Limit = limit, Date = date };
        const string sql = 
            """
            
                        SELECT DISTINCT 
                            p.id,
                            p.user_id as UserId,
                            p.content as Content,
                            l.id as LocationId,
                            p.views as Views,
                            p.hide_stats as HideStats,
                            p.hide_comments as HideComments,
                            p.created_at as CreatedAt,
                            p.updated_at as UpdatedAt,
                            pg.id as Id,
                            pg.file as File,
                            pg.description as Description,
                            pg.labels as Labels,
                            u.id as Id,
                            u.username as Username,
                            up.fullname as Fullname,
                            up.id as Id,
                            up.Image as Image,
                            t.id as Id,
                            t.name as Name,
                            pc.id as Id,
                            pl.post_id as PostId,
                            pl.user_id as UserId
                        FROM users_subscriptions us
                        INNER JOIN public.posts p on us.user_id = p.user_id
                        INNER JOIN posts_gallery pg ON p.id = pg.post_id
                        INNER JOIN public.users u on u.id = p.user_id
                        INNER JOIN public.users_profiles up on u.id = up.user_id
                        LEFT JOIN locations l ON p.location_id = l.id
                        LEFT JOIN public.posts_to_tags ptt on p.id = ptt.post_id
                        LEFT JOIN public.tags t on t.id = ptt.tag_id
                        LEFT JOIN public.posts_comments pc on p.id = pc.post_id
                        LEFT JOIN public.posts_likes pl on p.id = pl.post_id
                        WHERE
                            p.created_at < @date and
                            p.active = true and
                            us.subscriber_id = @subscriberId
                        ORDER BY p.created_at DESC
                        LIMIT @limit
                        OFFSET @offset
                        
            """;

        var postDictionary = new Dictionary<Guid, Post>();
        var postGalleries = new HashSet<Guid>();
        var postTags = new HashSet<Guid>();
        var postComments = new HashSet<Guid>();
        var postLikes = new HashSet<string>();
        
        var posts = await connection.QueryAsync<Post, PostGallery, User, UserProfile, Tag, PostComment, PostLike, Post>(
            sql,
            (post, gallery, user, profile, tag, comment, like) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out Post? postEntry))
                {
                    postEntry = post;
                    postDictionary.Add(postEntry.Id, postEntry);
                }
                
                postEntry.User = user;
                user.Profile = profile;

                if (!postGalleries.Contains(gallery.Id))
                {
                    postEntry.AddGallery(gallery);
                    postGalleries.Add(gallery.Id);
                }

                if (tag?.Name != null && !postTags.Contains(tag.Id))
                {
                    postEntry.AddTag(tag);
                    postTags.Add(tag.Id);
                }

                if (comment != null && comment.Id != Guid.Empty && !postComments.Contains(comment.Id))
                {
                    postEntry.CommentsCount += 1;
                    postComments.Add(comment.Id);
                }

                if (like != null && like.PostId != Guid.Empty && like.UserId != Guid.Empty && !postLikes.Contains($"{like.UserId}{like.PostId}"))
                {
                    postEntry.LikesCount += 1;
                    postLikes.Add($"{like.UserId}{like.PostId}");
                }                    
                
                return postEntry;
            },
            parameters,
            splitOn: "Id,Id,Id,Id,Id,PostId"
        );
        
        return posts.DistinctBy(x => x.Id).ToList();
    }

    public async Task<PostGallery?> GetGallery(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
            SELECT * FROM posts_gallery p
            WHERE p.id = @id
            ";

        var gallery = await connection.QueryFirstOrDefaultAsync<PostGallery>(sql, parameters);
        return gallery;
    }
    public async Task<List<PostGallery>> AllGalleries(Guid postId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = postId };
        const string sql = 
            @"
                SELECT * FROM posts_gallery p WHERE p.post_id = @id; 
            ";
        
        var galleries = await connection.QueryAsync<PostGallery>(sql, parameters);
        
        return galleries.ToList();
    }
    
    public async Task<PostComment?> GetComment(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
            SELECT * FROM posts_comments p
            WHERE p.id = @id
            ";

        var comment = await connection.QueryFirstOrDefaultAsync<PostComment>(sql, parameters);
        return comment;
    }
    public async Task<List<PostComment>> AllPostComments(Guid postId, int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { PostId = postId, Offset = offset, Limit = limit };
        const string sql = 
            @"
                WITH parents AS (
                    SELECT 
                        ip.id,
                        ip.content,
                        ip.user_id,
                        iu.username 
                    FROM posts_comments ip
                    INNER JOIN public.users iu ON iu.id = ip.user_id
                    WHERE ip.post_id = @postId AND ip.parent_id IS NULL
                    ORDER BY ip.created_at DESC            
                    LIMIT @limit
                    OFFSET @offset
                )

                SELECT 
                    p.id,
                    p.content,
                    0 as user_split,
                    p.user_id as Id,
                    p.username,
                    0 as child_split,
                    pc.id,
                    pc.content,
                    0 as child_user_split,
                    pc.user_id,
                    u.username  
                FROM parents p
                LEFT JOIN posts_comments pc ON pc.parent_id = p.id
                LEFT JOIN public.users u ON u.id = pc.user_id;

            ";

        var commentsDictionary = new Dictionary<Guid, PostComment>();
        var commentChildDictionary = new Dictionary<Guid, HashSet<Guid>>();
        var comments = await connection.QueryAsync<PostComment, User, PostComment, User, PostComment>(
            sql,
            (comment, user, child, childUser) =>
            {
                if (!commentsDictionary.TryGetValue(comment.Id, out PostComment? commentEntry))
                {
                    commentEntry = comment;
                    commentsDictionary.Add(commentEntry.Id, commentEntry);
                    commentChildDictionary.Add(commentEntry.Id, new HashSet<Guid>());
                }
                
                if (commentEntry.User == null)
                    commentEntry.User = user;
                
                if (child == null || child.Id == Guid.Empty)
                    return commentEntry;

                var childSet = commentChildDictionary[commentEntry.Id];
                if (!childSet.Contains(child.Id))
                {
                    child.User = childUser;
                    comment.AddChild(child);
                    childSet.Add(child.Id);
                }
                return comment;
            },
            parameters,
            splitOn: "user_split,child_split,child_user_split"
            );
        
        return comments.DistinctBy(x => x.Id).ToList();
    }
    public async Task<List<PostComment>> AllPostParentComments(Guid postId, int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { PostId = postId, Offset = offset, Limit = limit };
        const string sql = 
            @"
            SELECT
                p.id,
                p.content,
                0 as user_split,
                p.user_id as Id,
                u.username
            FROM posts_comments p
            INNER JOIN public.users u on u.id = p.user_id
            WHERE p.post_id = @postId and p.parent_id is null
            ORDER BY p.created_at DESC            
            LIMIT @limit
            OFFSET @offset
            ";

        var comments = await connection.QueryAsync<PostComment, User, PostComment>(
            sql,
            (comment, user) =>
            {
                comment.User = user;
                return comment;
            },
            parameters,
            splitOn: "user_split"
        );
        return comments.ToList();
    }
    public async Task<List<PostComment>> AllChildComments(Guid commentId, int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { CommentId = commentId, Offset = offset, Limit = limit };
        const string sql = 
            @"
            SELECT
                p.id,
                p.content,
                0 as user_split,
                p.user_id as Id,
                u.username
            FROM posts_comments p
            INNER JOIN public.users u on u.id = p.user_id
            WHERE p.parent_id = @commentId
            ORDER BY p.created_at DESC            
            LIMIT @limit
            OFFSET @offset
            ";

        var comments = await connection.QueryAsync<PostComment, User, PostComment>(
            sql,
            (comment, user) =>
            {
                comment.User = user;
                return comment;
            },
            parameters,
            splitOn: "user_split"
        );
        return comments.ToList();
    }
    
    public async Task<long> GetTotalPosts()
    {
        var connection = _context.CreateConnection();
        const string sql = "SELECT count(*) FROM posts;";
        return await connection.QueryFirstAsync<long>(sql);
    }
    public async Task<long> GetTotalPosts(DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Date = date };
        const string sql = "SELECT count(*) FROM posts WHERE created_at < @date;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
    
    public async Task<long> GetTotalUserPosts(Guid userId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId };
        const string sql = "SELECT count(*) FROM posts WHERE user_id = @userId;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
    public async Task<long> GetTotalUserPosts(Guid userId, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId, Date = date };
        const string sql = "SELECT count(*) FROM posts WHERE user_id = @userId and created_at < @date;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }

    public async Task<long> GetTotalHomePosts()
    {
        var connection = _context.CreateConnection();
        const string sql = "SELECT count(*) FROM posts;";
        return await connection.QueryFirstAsync<long>(sql);
    }
    public async Task<long> GetTotalHomePosts(DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Date = date };
        const string sql = "SELECT count(*) FROM posts WHERE created_at < @date;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }

    public async Task<long> GetTotalPostComments(Guid postId)
    {        
        var connection = _context.CreateConnection();
        var parameters = new { PostId = postId };
        const string sql = "SELECT count(*) FROM posts_comments WHERE post_id = @postId;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
    public async Task<long> GetTotalPostParentComments(Guid postId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { PostId = postId };
        const string sql = "SELECT count(*) FROM posts_comments WHERE post_id = @postId and parent_id is null;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
    public async Task<long> GetTotalChildComments(Guid commentId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { CommentId = commentId };
        const string sql = "SELECT count(*) FROM posts_comments WHERE parent_id = @commentId;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
    
    public async Task<bool> HasUserNewPosts(Guid userId, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId, Date = date };
        const string sql = 
            @"
            SELECT count(*) FROM posts p
            WHERE p.user_id = @userId and p.created_at > @date
            ";
        
        var count = await connection.QueryFirstAsync<long>(sql, parameters);
        return count > 0;
    }
    public async Task<bool> HasHomeNewPosts(DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Date = date };
        const string sql = 
            @"
            SELECT count(*) FROM posts p
            WHERE p.created_at > @date
            ";
        
        var count = await connection.QueryFirstAsync<long>(sql, parameters);
        return count > 0;
    }
}