using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Infrastructure.Persistence.EF.Repositories;

public class EfUserRepository : IEfUserRepository
{
    private readonly EfContext _context;

    public EfUserRepository(EfContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        await _context.SingleInsertAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        await _context.SingleUpdateAsync(user);
        await _context.SaveChangesAsync();
    }
}