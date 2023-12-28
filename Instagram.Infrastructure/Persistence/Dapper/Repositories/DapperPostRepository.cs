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
                p.location_id as LocationId, 
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

        var post = posts.FirstOrDefault();
        return post;
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

    public async Task<List<Post>> GetPosts(int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit };
        const string sql = 
            @"
            SELECT 
                p.id, 
                p.user_id as UserId, 
                p.content, 
                p.location_id as LocationId, 
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
            ORDER BY p.id
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

    public async Task<List<PostGallery>> GetPostGalleries(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            @"
                SELECT * FROM posts_gallery p WHERE p.post_id = @Id; 
            ";
        
        var galleries = await connection.QueryAsync<PostGallery>(sql, parameters);
        
        return galleries.ToList();
    }

    public Task<PostGallery?> GetGallery(Guid galleryId)
    {
        throw new NotImplementedException();
    }
}