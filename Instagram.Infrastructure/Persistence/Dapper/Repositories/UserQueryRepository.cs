using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Infrastructure.Persistence.Dapper.Common;

namespace Instagram.Infrastructure.Persistence.Dapper.Repositories;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly DapperContext _context;

    public UserQueryRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserById(long id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        var sql = @"
                       SELECT * FROM users u 
                       INNER JOIN profiles p ON p.user_id = u.id
                       WHERE u.id = @Id;
                   ";
        
        var users = await connection.QueryAsync<User, UserProfile, User>(sql, (user, profile) =>
        {
            user.SetProfile(profile);
            return user;
        }, parameters, splitOn: "id");

        return users.First();
    }

    public async Task<User?> GetUserByIdentity(string? username, string? email, string? phone)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Username = username, Email = email, Phone = phone };
        var sql = @"
            SELECT * FROM users
            WHERE 
                (@Username IS NOT NULL AND username = @Username)
                OR 
                (@Email IS NOT NULL AND email = @Email)
                OR 
                (@Phone IS NOT NULL AND phone = @Phone);
        ";

        return await connection.QuerySingleOrDefaultAsync<User>(sql, parameters);
    }
}