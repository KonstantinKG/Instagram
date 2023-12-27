using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByIdentity(string? username, string? email, string? phone);
    Task<List<User>> GetAllUsers(int offset, int limit);
    Task<long> GetTotalUsers();

    Task<UserGender?> GetUserGender(string name);
}