using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.EfRepositories;

public interface IEfPostRepository
{
    Task AddPost(Post post);
    Task UpdatePost(Post post);
    Task DeletePost(Post post);
    Task AddTags(List<Tag> tags);
    Task AddPostTags(List<PostToTag> tags);
    Task DeletePostTags(List<PostToTag> tags);
    Task AddGallery(PostGallery gallery);
    Task UpdateGallery(PostGallery gallery);
    Task DeleteGallery(PostGallery gallery);
    Task AddComment(PostComment comment);
    Task UpdateComment(PostComment comment);
    Task DeleteComment(PostComment comment);
}