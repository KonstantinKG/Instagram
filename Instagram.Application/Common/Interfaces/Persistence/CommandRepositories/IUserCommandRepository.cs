using System.Linq.Expressions;

using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.CommandRepositories;

public interface IUserCommandRepository
{
    Task AddUser(User user);

    Task UpdateUser(User user);
}