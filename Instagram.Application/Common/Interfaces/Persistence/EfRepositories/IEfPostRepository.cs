using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.EfRepositories;

public interface IEfPostRepository
{
    Task AddPost(Post post);
    Task UpdatePost(Post post);
    Task DeletePost(Post post);
    Task AddGallery(PostGallery gallery);
    Task UpdateGallery(PostGallery gallery);
    Task DeleteGallery(PostGallery gallery);
    Task AddComment(PostComment comment);
    Task UpdateComment(PostComment comment);
    Task DeleteComment(PostComment comment);
}