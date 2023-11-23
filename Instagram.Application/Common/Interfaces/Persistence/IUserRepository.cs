using System.Linq.Expressions;

using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserBy(Expression<Func<User, bool>> predicate);
    Task AddUser(User user);
}