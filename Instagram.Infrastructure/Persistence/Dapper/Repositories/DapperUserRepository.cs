using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.Dapper.Repositories;

public class DapperUserRepository : IDapperUserRepository
{
    private readonly DapperContext _context;

    public DapperUserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserById(string id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            """
                SELECT * FROM users u
                INNER JOIN profiles p ON p.user_id = u.id
                WHERE u.id = @Id;
            """;
        
        var users = await connection.QueryAsync<User, UserProfile, User>(sql, (user, profile) =>
        {
            user.SetProfile(profile);
            return user;
        }, parameters, splitOn: "id");

        return users.FirstOrDefault();
    }

    public async Task<User?> GetUserByIdentity(string? username, string? email, string? phone)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Username = username, Email = email, Phone = phone };
        const string sql = 
            """
               SELECT * FROM users
               WHERE
                   (@Username IS NOT NULL AND username = @Username)
                   OR
                   (@Email IS NOT NULL AND email = @Email)
                   OR
                   (@Phone IS NOT NULL AND phone = @Phone);
           """;

        return await connection.QuerySingleOrDefaultAsync<User>(sql, parameters);
    }

    public async Task<List<User>> GetAllUsers(int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit };
        const string sql = 
            @"
                SELECT * FROM users u
                INNER JOIN profiles p ON p.user_id = u.id
                ORDER BY u.id
                LIMIT @limit
                OFFSET @offset
            ";
        
        var users = await connection.QueryAsync<User, UserProfile, User>(
            sql,
            (user, profile) =>
            {
                user.SetProfile(profile);
                return user;
            }, 
            parameters,
            splitOn: "id"
        );
        
        return users.ToList();
    }
}