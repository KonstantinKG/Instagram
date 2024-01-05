using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;

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

    public async Task AddTags(List<Tag> tags)
    {
        await _context.BulkInsertAsync(tags);
        await _context.SaveChangesAsync();
    }

    public async Task AddPostTags(List<PostToTag> tags)
    {
        await _context.AddRangeAsync(tags);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePostTags(List<PostToTag> tags)
    {
        await _context.BulkDeleteAsync(tags);
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
    
    public async Task DeleteGallery(PostGallery gallery)
    {
        await _context.SingleDeleteAsync(gallery);
        await _context.SaveChangesAsync();
    }

    public async Task AddComment(PostComment comment)
    {
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateComment(PostComment comment)
    {
        await _context.SingleUpdateAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(PostComment comment)
    {
        await _context.SingleDeleteAsync(comment);
        await _context.SaveChangesAsync();
    }
}