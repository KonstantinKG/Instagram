using Instagram.Application.Services.PostService.Commands.AddPost;
using Instagram.Application.Services.PostService.Commands.AddPostComment;
using Instagram.Application.Services.PostService.Commands.AddPostGallery;
using Instagram.Application.Services.PostService.Commands.DeletePost;
using Instagram.Application.Services.PostService.Commands.DeletePostComment;
using Instagram.Application.Services.PostService.Commands.DeletePostGallery;
using Instagram.Application.Services.PostService.Commands.UpdatePost;
using Instagram.Application.Services.PostService.Commands.UpdatePostComment;
using Instagram.Application.Services.PostService.Commands.UpdatePostGallery;
using Instagram.Application.Services.PostService.Commands.UpdatePostStatus;
using Instagram.Application.Services.PostService.Queries.AllHomePosts;
using Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;
using Instagram.Application.Services.PostService.Queries.AllPostComments;
using Instagram.Application.Services.PostService.Queries.AllPostParentComments;
using Instagram.Application.Services.PostService.Queries.AllPosts;
using Instagram.Application.Services.PostService.Queries.AllUserPosts;
using Instagram.Application.Services.PostService.Queries.GetHomeNewPostsStatus;
using Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;
using Instagram.Application.Services.PostService.Queries.GetPost;
using Instagram.Application.Services.PostService.Queries.GetUserNewPostsStatus;
using Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;
using Instagram.Contracts.Post._Common;
using Instagram.Contracts.Post.AddPost;
using Instagram.Contracts.Post.AddPostComment;
using Instagram.Contracts.Post.AddPostGallery;
using Instagram.Contracts.Post.AllHomePosts;
using Instagram.Contracts.Post.AllPostCommentChildren;
using Instagram.Contracts.Post.AllPostComments;
using Instagram.Contracts.Post.AllPostParentComments;
using Instagram.Contracts.Post.AllPosts;
using Instagram.Contracts.Post.AllUserPosts;
using Instagram.Contracts.Post.DeletePost;
using Instagram.Contracts.Post.DeletePostComment;
using Instagram.Contracts.Post.DeletePostGallery;
using Instagram.Contracts.Post.GetHomeNewPostsStatus;
using Instagram.Contracts.Post.GetHomePostsSlider;
using Instagram.Contracts.Post.GetUserNewPostsStatus;
using Instagram.Contracts.Post.GetUserPostsSlider;
using Instagram.Contracts.Post.UpdatePost;
using Instagram.Contracts.Post.UpdatePostComment;
using Instagram.Contracts.Post.UpdatePostGallery;
using Instagram.Contracts.Post.UpdatePostStatus;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class PostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        RegisterPostMappings(config);
        RegisterPostGalleryMappings(config);
        RegisterPostCommentMappings(config);
        RegisterUserPostMappings(config);
        RegisterHomePostMappings(config);
    }

    private void RegisterPostMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Post, PostResponse>()
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.content, src => src.Content)
            .Map(dest => dest.location_id, src => src.LocationId)
            .Map(dest => dest.views, src => src.Views)
            .Map(dest => dest.hide_stats, src => src.HideStats)
            .Map(dest => dest.hide_comments, src => src.HideComments)
            .Map(dest => dest.comments_count, src => src.CommentsCount)
            .Map(dest => dest.likes_count, src => src.LikesCount)
            .Map(dest => dest.created_at, src => src.CreatedAt)
            .Map(dest => dest.updated_at, src => src.UpdatedAt)
            .Map(dest => dest.galleries, src => src.Galleries)
            .Map(dest => dest.tags, src => src.Tags)
            .Map(dest => dest.user, src => src.User);

        config.NewConfig<PostGallery, PostGalleryResponse>()
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.file, src => src.File)
            .Map(dest => dest.description, src => src.Description)
            .Map(dest => dest.labels, src => src.Labels);
        
        config.NewConfig<Tag, PostTagResponse>()
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.name, src => src.Name);
        
        config.NewConfig<AddPostResult, AddPostResponse>()
            .Map(dest => dest.id, src => src.Id);

        config.NewConfig<(Guid userId, UpdatePostRequest request), UpdatePostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id)
            .Map(dest => dest.Content, src => src.request.content)
            .Map(dest => dest.LocationId, src => src.request.location_id)
            .Map(dest => dest.HideStats, src => src.request.hide_stats)
            .Map(dest => dest.HideComments, src => src.request.hide_comments)
            .Map(dest => dest.Tags, src => src.request.tags);
        
        config.NewConfig<AllPostsRequest, AllPostsQuery>()
            .Map(dest => dest.Page, src => src.page)
            .Map(dest => dest.Date, src => src.date);
        
        config.NewConfig<AllPostsResult, AllPostsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.posts, src => src.Posts);
        
        config.NewConfig<(Guid userId, UpdatePostStatusRequest request), UpdatePostStatusCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id);
        
        config.NewConfig<(Guid userId, DeletePostRequest request), DeletePostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id);
    }
    
    private void RegisterPostGalleryMappings(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid userId, AddPostGalleryRequest request), AddPostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.PostId, src => src.request.post_id)
            .Map(dest => dest.File, src => new AppFileProxy(src.request.file))
            .Map(dest => dest.Description, src => src.request.description)
            .Map(dest => dest.Labels, src => src.request.labels);
        
        config.NewConfig<(Guid userId, UpdatePostGalleryRequest request), UpdatePostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id)
            .Map(dest => dest.PostId, src => src.request.post_id)
            .Map(dest => dest.File, src => new AppFileProxy(src.request.file))
            .Map(dest => dest.Description, src => src.request.description)
            .Map(dest => dest.Labels, src => src.request.labels);
        
        config.NewConfig<(Guid userId, DeletePostGalleryRequest request), DeletePostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id)
            .Map(dest => dest.PostId, src => src.request.post_id);
    }
    
    private void RegisterPostCommentMappings(TypeAdapterConfig config)
    {
        config.NewConfig<PostComment, PostCommentResponse>()
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.content, src => src.Content)
            .Map(dest => dest.user, src => src.User)
            .Map(dest => dest.comments, src => src.Comments);
        
        config.NewConfig<AllPostCommentsRequest, AllPostCommentsQuery>()
            .Map(dest => dest.Page, src => src.page)
            .Map(dest => dest.PostId, src => src.post_id);
        
        config.NewConfig<AllPostCommentsResult, AllPostCommentsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.comments, src => src.Comments);
        
        config.NewConfig<AllPostParentCommentsRequest, AllPostParentCommentsQuery>()
            .Map(dest => dest.PostId, src => src.post_id)
            .Map(dest => dest.Page, src => src.page);
        
        config.NewConfig<AllPostParentCommentsResult, AllPostParentCommentsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.comments, src => src.Comments);
        
        config.NewConfig<AllPostCommentChildrenRequest, AllPostCommentChildrenQuery>()
            .Map(dest => dest.CommentId, src => src.comment_id)
            .Map(dest => dest.Page, src => src.page);
        
        config.NewConfig<AllPostCommentChildrenResult, AllPostCommentChildrenResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.comments, src => src.Comments);
        
        config.NewConfig<(Guid userId, AddPostCommentRequest request), AddPostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.PostId, src => src.request.post_id)
            .Map(dest => dest.ParentId, src => src.request.parent_id)
            .Map(dest => dest.Content, src => src.request.content);
        
        config.NewConfig<(Guid userId, UpdatePostCommentRequest request), UpdatePostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id)
            .Map(dest => dest.Content, src => src.request.content);
        
        config.NewConfig<(Guid userId, DeletePostCommentRequest request), DeletePostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.request.id);
    }
    
    private void RegisterUserPostMappings(TypeAdapterConfig config)
    {
        config.NewConfig< (Guid userId, AllUserPostsRequest request), AllUserPostsQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Page, src => src.request.page)
            .Map(dest => dest.Date, src => src.request.date);
        
        config.NewConfig<AllUserPostsResult, AllUserPostsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.posts, src => src.Posts);
        
        config.NewConfig< (Guid userId, GetUserPostsSliderRequest request), GetUserPostsSliderQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Page, src => src.request.page)
            .Map(dest => dest.Date, src => src.request.date)
            .Map(dest => dest.PostId, src => src.request.post_id);

        config.NewConfig<GetUserPostsSliderResult, GetUserPostsSliderResponse>()
            .Map(dest => dest.previous, src => src.Previous)
            .Map(dest => dest.next, src => src.Next)
            .Map(dest => dest.post, src => src.Post);
        
        config.NewConfig<GetUserNewPostsStatusRequest, GetUserNewPostsStatusQuery>()
            .Map(dest => dest.UserId, src => src.user_id)
            .Map(dest => dest.Date, src => src.date);

        config.NewConfig<GetUserNewPostsStatusResult, GetUserNewPostsStatusResponse>()
            .Map(dest => dest.status, src => src.Status);
    }
    
    private void RegisterHomePostMappings(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid userId, AllHomePostsRequest request), AllHomePostsQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Page, src => src.request.page)
            .Map(dest => dest.Date, src => src.request.date);

        config.NewConfig<AllHomePostsResult, AllHomePostsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.posts, src => src.Posts);
        
        config.NewConfig<(Guid userId, GetHomePostsSliderRequest request), GetHomePostsSliderQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Page, src => src.request.page)
            .Map(dest => dest.Date, src => src.request.date)
            .Map(dest => dest.PostId, src => src.request.post_id);

        config.NewConfig<GetHomePostsSliderResult, GetHomePostsSliderResponse>()
            .Map(dest => dest.previous, src => src.Previous)
            .Map(dest => dest.next, src => src.Next)
            .Map(dest => dest.post, src => src.Post);
        
        config.NewConfig<GetHomeNewPostsStatusRequest, GetHomeNewPostsStatusQuery>()
            .Map(dest => dest.Date, src => src.date);

        config.NewConfig<GetHomeNewPostsStatusResult, GetHomeNewPostsStatusResponse>()
            .Map(dest => dest.status, src => src.Status);
    }
}
