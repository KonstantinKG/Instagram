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
            """
                SELECT 
                    p.id, 
                    p.user_id as UserId, 
                    p.content, 
                    p.location_id as LocationId, 
                    p.views, 
                    p.hide_stats as HideStats, 
                    p.hide_comments as HideComments, 
                    p.created_at as CreatedAt, 
                    p.updated_at as UpdatedAt
                FROM posts p
                LEFT JOIN locations l ON p.location_id = l.id
                WHERE p.id = @Id;
            """;

        var post = await connection.QueryFirstOrDefaultAsync<Post>(sql, parameters);
        return post;
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
                    p.updated_at as UpdatedAt
                FROM posts p
                LEFT JOIN locations l ON p.location_id = l.id
                ORDER BY p.id
                LIMIT @limit
                OFFSET @offset
            ";
        
        var posts = await connection.QueryAsync<Post>(sql, parameters);
        
        return posts.ToList();
    }

    public Task<PostGallery?> GetGallery(Guid galleryId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostGallery>> GetGalleries(int offset, int limit)
    {
        throw new NotImplementedException();
    }
}