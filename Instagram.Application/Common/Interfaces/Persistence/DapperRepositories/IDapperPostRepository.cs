using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperPostRepository
{
    Task<Post?> GetPost(Guid id);
    Task<Post?> GetPostWithGallery(Guid id);
    
    Task<List<Post>> AllPosts(int offset, int limit, DateTime date);
    Task<List<Post>> AllUserPosts(Guid userId, int offset, int limit, DateTime date);
    Task<List<Post>> AllHomePosts(int offset, int limit, DateTime date);

    Task<PostGallery?> GetGallery(Guid id);
    Task<List<PostGallery>> AllGalleries(Guid postId);
    
    Task<PostComment?> GetComment(Guid id);
    Task<List<PostComment>> AllPostComments(Guid postId, int offset, int limit);
    Task<List<PostComment>> AllPostParentComments(Guid postId, int offset, int limit);
    Task<List<PostComment>> AllChildComments(Guid commentId, int offset, int limit);
    
    Task<long> GetTotalPosts();
    Task<long> GetTotalPosts(DateTime date);
    
    Task<long> GetTotalUserPosts(Guid userId);
    Task<long> GetTotalUserPosts(Guid userId, DateTime date);
    
    Task<long> GetTotalHomePosts();
    Task<long> GetTotalHomePosts(DateTime date);
    
    Task<long> GetTotalPostComments(Guid postId);
    Task<long> GetTotalPostParentComments(Guid postId);
    Task<long> GetTotalChildComments(Guid commentId);
    
    Task<bool> HasUserNewPosts(Guid userId, DateTime date);
    Task<bool> HasHomeNewPosts(DateTime date);
}