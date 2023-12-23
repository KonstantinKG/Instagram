using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.EF.Repositories;

public class EfPostRepository : IEfPostRepository
{
    private readonly EfContext _context;

    public EfPostRepository(EfContext context)
    {
        _context = context;
    }

    async public Task AddPost(Post post, List<PostGallery> galleries)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.SingleInsertAsync(post);
            await _context.BulkInsertAsync(galleries);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}