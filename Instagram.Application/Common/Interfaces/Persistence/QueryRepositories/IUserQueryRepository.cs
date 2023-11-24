using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;

public interface IUserQueryRepository
{
    Task<User?> GetUserById(long id);
    Task<User?> GetUserByIdentity(string? username, string? email, string? phone);
    Task<List<User>> GetAllUsers();
}