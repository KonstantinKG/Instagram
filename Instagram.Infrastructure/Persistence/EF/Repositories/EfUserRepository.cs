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

    public async Task AddUser(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SingleInsertAsync(user);
            await _context.SingleInsertAsync(user.Profile);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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