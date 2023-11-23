using System.Linq.Expressions;

using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Domain.Aggregates.UserAggregate;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InstagramDbContext _dbContext;

    public UserRepository(InstagramDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserBy(Expression<Func<User,bool>> predicate)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(predicate);
    }

    public async Task AddUser(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}