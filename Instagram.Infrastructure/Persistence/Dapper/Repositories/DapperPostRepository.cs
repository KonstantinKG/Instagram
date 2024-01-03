using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

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
            WHERE p.id = @Id
            ";

        var post = await connection.QueryFirstOrDefaultAsync<Post>(sql, parameters);
        return post;
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
            WHERE p.id = @Id
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

    public async Task<PostGallery?> GetGallery(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
            SELECT * FROM posts_gallery p
            WHERE p.id = @Id
            ";

        var gallery = await connection.QueryFirstOrDefaultAsync<PostGallery>(sql, parameters);
        return gallery;
    }

    public async Task<List<PostGallery>> GetGalleries(Guid postId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = postId };
        const string sql = 
            @"
                SELECT * FROM posts_gallery p WHERE p.post_id = @Id; 
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
            WHERE p.id = @Id
            ";

        var comment = await connection.QueryFirstOrDefaultAsync<PostComment>(sql, parameters);
        return comment;
    }
    
    public async Task<List<PostComment>> GetParentComments(Guid postId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = postId };
        const string sql = 
            @"
            SELECT * FROM posts_comments p
            WHERE p.post_id = @Id
            ";
        
        var comments = await connection.QueryAsync<PostComment>(sql, parameters);
        return comments.ToList();
    }

    public async Task<List<PostComment>> GetChildComments(Guid commentId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = commentId };
        const string sql = 
            @"
            SELECT * FROM posts_comments p
            WHERE p.parent_id = @Id
            ";
        
        var comments = await connection.QueryAsync<PostComment>(sql, parameters);
        return comments.ToList();
    }
    
    public async Task<long> GetTotalPosts()
    {
        var connection = _context.CreateConnection();
        const string sql = 
            """
                SELECT count(*) FROM posts;
            """;

        return await connection.QueryFirstAsync<long>(sql);
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