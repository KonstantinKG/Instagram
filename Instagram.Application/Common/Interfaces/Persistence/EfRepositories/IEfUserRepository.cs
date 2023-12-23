using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.EfRepositories;

public interface IEfUserRepository
{
    Task AddUser(User user);

    Task UpdateUser(User user);
}