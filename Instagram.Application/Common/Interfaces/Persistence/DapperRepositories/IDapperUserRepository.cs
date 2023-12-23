﻿using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperUserRepository
{
    Task<User?> GetUserById(string id);
    Task<User?> GetUserByIdentity(string? username, string? email, string? phone);
    Task<List<User>> GetAllUsers(int offset, int limit);
}