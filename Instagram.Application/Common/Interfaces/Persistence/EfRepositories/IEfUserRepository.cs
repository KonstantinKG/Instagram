using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.EfRepositories;

public interface IEfUserRepository
{
    Task AddUser(User user);

    Task UpdateUser(User? user, UserProfile? profile);

    Task AddUserGender(UserGender gender);
}