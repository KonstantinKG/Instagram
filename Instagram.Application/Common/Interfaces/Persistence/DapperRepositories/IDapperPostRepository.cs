using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperPostRepository
{
    Task<Post?> GetPost(Guid id);
    Task<List<Post>> AllPosts(int offset, int limit, DateTime date);
    Task<List<Post>> AllUserPosts(Guid userId, int offset, int limit, DateTime date);
    Task<List<Post>> AllHomePosts(int offset, int limit, DateTime date);
    Task<Post?> GetPostWithGallery(Guid id);
    Task<PostGallery?> GetGallery(Guid id);
    Task<List<PostGallery>> GetGalleries(Guid postId);
    Task<PostComment?> GetComment(Guid id);
    Task<List<PostComment>> GetParentComments(Guid postId);
    Task<List<PostComment>> GetChildComments(Guid commentId);
    
    Task<long> GetTotalPosts();
    Task<bool> HasUserNewPosts(Guid userId, DateTime date);
    Task<bool> HasHomeNewPosts(DateTime date);
}