using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.CommandRepositories;
using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Infrastructure.Persistence.EF.Repositories;

public class UserCommandRepository : IUserCommandRepository
{
    private readonly EfContext _context;

    public UserCommandRepository(EfContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }
}