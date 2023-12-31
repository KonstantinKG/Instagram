﻿using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;

public interface IDapperPostRepository
{
    Task<Post?> GetPost(Guid id);
    Task<Post?> GetPostWithGalleries(Guid id);
    Task<Post?> GetFullPost(Guid id);
    
    Task<Tag?> GetTag(string name);
    Task<List<Tag>> GetPostTags(Guid id);
    
    Task<List<Post>> AllPosts(int offset, int limit, DateTime date);
    Task<List<Post>> AllUserPosts(Guid userId, int offset, int limit, DateTime date);
    Task<List<Post>> AllHomePosts(Guid subscriberId, int offset, int limit, DateTime date);

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