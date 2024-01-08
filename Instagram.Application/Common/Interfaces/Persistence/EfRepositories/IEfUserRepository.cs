using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.EfRepositories;

public interface IEfUserRepository
{
    Task UpdateUserProfile(UserProfile profile);
    Task AddUserGender(UserGender gender);
    Task AddUserSubscription(UserSubscription subscription);
    Task DeleteUserSubscription(UserSubscription subscription);
}