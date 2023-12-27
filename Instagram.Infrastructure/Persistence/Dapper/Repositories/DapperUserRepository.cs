﻿using Dapper;

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
                user.SetProfile(profile);
                return user;
            }, 
            parameters,
            splitOn: "id"
        );
        
        return users.ToList();
    }

    public async Task<long> GetTotalUsers()
    {
        var connection = _context.CreateConnection();
        const string sql = 
            """
                SELECT count(*) FROM users;
            """;

        return await connection.QueryFirstAsync<long>(sql);
        
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
        
        var gender = await connection.QueryFirstOrDefaultAsync<UserGender>(
            sql,
            parameters
        );
        
        return gender;
    }
}