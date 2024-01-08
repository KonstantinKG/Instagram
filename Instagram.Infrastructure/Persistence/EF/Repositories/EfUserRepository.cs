using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.EF.Repositories;

public class EfUserRepository : IEfUserRepository
{
    private readonly EfContext _context;

    public EfUserRepository(EfContext context)
    {
        _context = context;
    }

    public async Task UpdateUserProfile(UserProfile profile)
    {
        await _context.SingleUpdateAsync(profile);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddUserGender(UserGender gender)
    {
        await _context.AddAsync(gender);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserSubscription(UserSubscription subscription)
    {
        await _context.SingleInsertAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserSubscription(UserSubscription subscription)
    {
        await _context.SingleDeleteAsync(subscription);
        await _context.SaveChangesAsync();
    }
}