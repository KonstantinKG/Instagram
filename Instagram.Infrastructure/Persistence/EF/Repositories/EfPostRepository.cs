using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

using Microsoft.EntityFrameworkCore;

namespace Instagram.Infrastructure.Persistence.EF.Repositories;

public class EfPostRepository : IEfPostRepository
{
    private readonly EfContext _context;

    public EfPostRepository(EfContext context)
    {
        _context = context;
    }

    public async Task AddPost(Post post)
    {
        await _context.SingleInsertAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePost(Post post)
    {
        await _context.SingleUpdateAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePost(Post post)
    {
        await _context.SingleDeleteAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task AddGallery(PostGallery gallery)
    {
        await _context.SingleInsertAsync(gallery);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGallery(PostGallery gallery)
    {
        await _context.SingleUpdateAsync(gallery);
        await _context.SaveChangesAsync();
    }
}