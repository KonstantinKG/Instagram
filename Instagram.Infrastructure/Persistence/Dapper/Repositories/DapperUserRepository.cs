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

    public async Task<User?> GetUserById(Guid id)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Id = id };
        const string sql = 
            """
                SELECT * FROM users u
                INNER JOIN users_profiles p ON p.user_id = u.id
                LEFT JOIN users_genders g ON p.gender_id = g.id
                WHERE u.id = @Id;
            """;
        
        var users = await connection.QueryAsync<User, UserProfile, UserGender, User>(sql, (user, profile, gender) =>
        {
            user.Profile = profile;
            profile.Gender = gender;
            return user;
        }, parameters, splitOn: "id, gender_id");

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

    public async Task<UserProfile?> GetUserProfile(Guid userId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId };
        const string sql = "SELECT * FROM users_profiles p WHERE p.user_id = @userId;";
        var profile = await connection.QuerySingleOrDefaultAsync<UserProfile>(sql,parameters);
        return profile;
    }

    public async Task<UserSubscription?> GetUserSubscription(Guid subscriberId, Guid userId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { SubscriberId = subscriberId, UserId = userId};
        const string sql = 
            """
                SELECT
                    us.subscriber_id as SubscriberId,
                    us.user_id as UserId,
                    us.created_at as CreatedAt
                FROM users_subscriptions us
                WHERE us.subscriber_id = @subscriberId AND us.user_id = @userId;
            """;

        return await connection.QuerySingleOrDefaultAsync<UserSubscription>(sql, parameters);
    }

    public async Task<List<User>> GetAllUsers(int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Offset = offset, Limit = limit };
        const string sql = 
            @"
                SELECT * FROM users u
                INNER JOIN users_profiles p ON p.user_id = u.id
                ORDER BY u.id
                LIMIT @limit
                OFFSET @offset
            ";
        
        var users = await connection.QueryAsync<User, UserProfile, User>(
            sql,
            (user, profile) =>
            {
                user.Profile = profile;
                return user;
            }, 
            parameters,
            splitOn: "id"
        );
        
        return users.ToList();
    }

    public async Task<List<User>> GetAllUserSubscriptions(Guid subscriberId, int offset, int limit)
    {
        var connection = _context.CreateConnection();
        var parameters = new { SubscriberId = subscriberId, Offset = offset, Limit = limit };
        const string sql = 
            """
                SELECT
                    u.id,
                    u.username,
                    p.fullname,
                    0 as profile_split,
                    p.image
                FROM users_subscriptions us
                INNER JOIN users u ON u.id = us.user_id 
                INNER JOIN users_profiles p ON p.user_id = u.id
                WHERE us.subscriber_id = @subscriberId
                ORDER BY us.created_at DESC 
                LIMIT @limit
                OFFSET @offset;
            """;

        var subscriptions = await connection.QueryAsync<User, UserProfile, User>(
            sql,
            (user, profile) =>
            {
                user.Profile = profile;
                return user;
            }, 
            parameters,
            splitOn: "profile_split"
        );
        
        return subscriptions.ToList();
    }

    public async Task<UserGender?> GetUserGender(string name)
    {
        var connection = _context.CreateConnection();
        var parameters = new { Name = name.ToLower() };
        const string sql = 
            @"
                SELECT * FROM users_genders
                WHERE lower(name) = @Name;
            ";
        
        var gender = await connection.QueryFirstOrDefaultAsync<UserGender>(sql,parameters);
        return gender;
    }
    
    public async Task<long> GetTotalUsers()
    {
        var connection = _context.CreateConnection();
        const string sql = "SELECT count(*) FROM users;";
        return await connection.QueryFirstAsync<long>(sql);
    }

    public async Task<long> GetTotalUserSubscriptions(Guid subscriberId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { SubscriberId = subscriberId };
        const string sql = "SELECT count(*) FROM users_subscriptions us WHERE us.subscriber_id = @subscriberId;";
        return await connection.QueryFirstAsync<long>(sql, parameters);
    }
}