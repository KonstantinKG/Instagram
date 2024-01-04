using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.UserAggregate;

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
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            WHERE p.id = @id
            ";

        var post = await connection.QueryFirstOrDefaultAsync<Post>(sql, parameters);
        return post;
    }
    public async Task<Post?> GetPostWithGallery(Guid id)
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
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt,
                0 as gallery_split,
                pg.id,
                pg.file as File,
                pg.description as Description,
                pg.labels as Labels
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            INNER JOIN posts_gallery pg ON p.id = pg.post_id
            WHERE p.id = @id
            ";

        var posts = await connection.QueryAsync<Post, PostGallery, Post>(
            sql,
            (post, gallery) =>
            {
                post.AddGallery(gallery);
                return post;
            },
            parameters,
            splitOn: "gallery_split"
        );
        
        return posts.FirstOrDefault();
    }

    public async Task<List<Post>> AllPosts(int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit, Date = date };
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
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt,
                0 as gallery_split,
                pg.id,
                pg.file as File,
                pg.description as Description,
                pg.labels as Labels
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            INNER JOIN posts_gallery pg ON p.id = pg.post_id
            WHERE p.created_at < @date
            ORDER BY p.created_at DESC
            LIMIT @limit
            OFFSET @offset
            ";

        var posts = await connection.QueryAsync<Post, PostGallery, Post>(
            sql,
            (post, gallery) =>
            {
                post.AddGallery(gallery);
                return post;
            },
            parameters,
            splitOn: "gallery_split"
        );
        
        return posts.ToList();
    }
    public async Task<List<Post>> AllUserPosts(Guid userId, int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId, Offset = offset, Limit = limit, Date = date };
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
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt,
                0 as gallery_split,
                pg.id,
                pg.file as File,
                pg.description as Description,
                pg.labels as Labels
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            INNER JOIN posts_gallery pg ON p.id = pg.post_id
            WHERE p.user_id = @userId and p.created_at < @date
            ORDER BY p.created_at DESC 
            LIMIT @limit
            OFFSET @offset
            ";

        var posts = await connection.QueryAsync<Post, PostGallery, Post>(
            sql,
            (post, gallery) =>
            {
                post.AddGallery(gallery);
                return post;
            },
            parameters,
            splitOn: "gallery_split"
        );
        
        return posts.ToList();
    }
    public async Task<List<Post>> AllHomePosts(int offset, int limit, DateTime date)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit, Date = date };
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
                p.created_at as CreatedAt, 
                p.updated_at as UpdatedAt,
                0 as gallery_split,
                pg.id,
                pg.file as File,
                pg.description as Description,
                pg.labels as Labels
            FROM posts p
            LEFT JOIN locations l ON p.location_id = l.id
            INNER JOIN posts_gallery pg ON p.id = pg.post_id
            WHERE p.created_at < @date
            ORDER BY p.created_at DESC
            LIMIT @limit
            OFFSET @offset
            ";

        var posts = await connection.QueryAsync<Post, PostGallery, Post>(
            sql,
            (post, gallery) =>
            {
                post.AddGallery(gallery);
                return post;
            },
            parameters,
            splitOn: "gallery_split"
        );
        
        return posts.ToList();
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
                INNER JOIN posts_comments pc ON pc.parent_id = p.id
                INNER JOIN public.users u ON u.id = pc.user_id;

            ";

        var comments = await connection.QueryAsync<PostComment, User, PostComment, User, PostComment>(
            sql,
            (comment, user, child, childUser) =>
            {
                comment.User = user;
                child.User = childUser;
                comment.AddChild(child);
                return comment;
            },
            parameters,
            splitOn: "user_split,child_split,child_user_split"
            );
        return comments.ToList();
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