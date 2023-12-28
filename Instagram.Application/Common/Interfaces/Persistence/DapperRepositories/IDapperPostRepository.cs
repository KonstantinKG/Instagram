using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperPostRepository
{
    Task<Post?> GetPost(Guid id);
    Task<long> GetTotalPosts();
    Task<List<Post>> GetPosts(int offset, int limit);
    Task<List<PostGallery>> GetPostGalleries(Guid id);
    Task<PostGallery?> GetGallery(Guid galleryId);
}