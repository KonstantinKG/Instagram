using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

using Microsoft.EntityFrameworkCore;

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
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User? user, UserProfile? profile)
    {
        if (user != null)
            await _context.SingleUpdateAsync(user);
        
        if (user != null)
            await _context.SingleUpdateAsync(profile);
        
        if (_context.ChangeTracker.HasChanges())
            await _context.SaveChangesAsync();
    }

    public async Task AddUserGender(UserGender gender)
    {
        await _context.AddAsync(gender);
        await _context.SaveChangesAsync();
    }
}