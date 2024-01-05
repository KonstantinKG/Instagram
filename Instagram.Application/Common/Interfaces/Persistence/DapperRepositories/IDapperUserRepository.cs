using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByIdentity(string? username, string? email, string? phone);
    Task<UserSubscription?> GetUserSubscription(Guid subscriberId, Guid userId);
    Task<List<User>> GetAllUsers(int offset, int limit);
    Task<List<User>> GetAllUserSubscriptions(Guid subscriberId, int offset, int limit);
    Task<UserGender?> GetUserGender(string name);
    
    Task<long> GetTotalUsers();
    Task<long> GetTotalUserSubscriptions(Guid subscriberId);
    
}
