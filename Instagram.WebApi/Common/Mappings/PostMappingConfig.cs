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
using Instagram.Contracts.Post.AddPost;
using Instagram.Contracts.Post.AddPostComment;
using Instagram.Contracts.Post.AddPostGallery;
using Instagram.Contracts.Post.AllHomePosts;
using Instagram.Contracts.Post.AllPostCommentChildren;
using Instagram.Contracts.Post.AllPostComments;
using Instagram.Contracts.Post.AllPostParentComments;
using Instagram.Contracts.Post.AllPosts;
using Instagram.Contracts.Post.AllUserPosts;
using Instagram.Contracts.Post.Common;
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
using Instagram.Domain.Aggregates.PostAggregate.Entities;
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
        config.NewConfig<AddPostResult, AddPostResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid userId, UpdatePostRequest request), EditPostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<GetPostResult, PostResponse>()
            .Map(dest => dest, src => src.Post);
        
        config.NewConfig<AllPostsRequest, AllPostsQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostsResult, AllPostsResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid userId, UpdatePostStatusRequest request), UpdatePostStatusCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, DeletePostRequest request), DeletePostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
    
    private void RegisterPostGalleryMappings(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid userId, AddPostGalleryRequest request), AddPostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.File , src => new AppFileProxy(src.request.File))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, UpdatePostGalleryRequest request), UpdatePostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.File , src => new AppFileProxy(src.request.File))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, DeletePostGalleryRequest request), DeletePostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
    
    private void RegisterPostCommentMappings(TypeAdapterConfig config)
    {
        config.NewConfig<PostComment, PostCommentResponse>()
            .Map(dest => dest.User, src => src.User)
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostCommentsRequest, AllPostCommentsQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostCommentsResult, AllPostCommentsResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<PostComment, AllPostCommentsComment>()
            .Map(dest => dest.User, src => src.User)
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostParentCommentsRequest, AllPostParentCommentsQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostParentCommentsResult, AllPostParentCommentsResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostCommentChildrenRequest, AllPostCommentChildrenQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<AllPostCommentChildrenResult, AllPostCommentChildrenResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid userId, AddPostCommentRequest request), AddPostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, UpdatePostCommentRequest request), UpdatePostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, DeletePostCommentRequest request), DeletePostCommentCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
    
    private void RegisterUserPostMappings(TypeAdapterConfig config)
    {
        config.NewConfig< (Guid userId, AllUserPostsRequest request), AllUserPostsQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<AllUserPostsResult, AllUserPostsRequest>()
            .Map(dest => dest, src => src);
        
        config.NewConfig< (Guid userId, GetUserPostsSliderRequest request), GetUserPostsSliderQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);

        config.NewConfig<GetUserPostsSliderResult, GetUserPostsSliderResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig< (Guid userId, GetUserNewPostsStatusRequest request), GetUserNewPostsStatusQuery>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);

        config.NewConfig<GetUserNewPostsStatusResult, GetUserNewPostsStatusResponse>()
            .Map(dest => dest, src => src);
    }
    
    private void RegisterHomePostMappings(TypeAdapterConfig config)
    {
        config.NewConfig<AllHomePostsRequest, AllHomePostsQuery>()
            .Map(dest => dest, src => src);

        config.NewConfig<AllHomePostsResult, AllHomePostsResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetHomePostsSliderRequest, GetHomePostsSliderQuery>()
            .Map(dest => dest, src => src);

        config.NewConfig<GetHomePostsSliderResult, GetHomePostsSliderResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetHomeNewPostsStatusRequest, GetHomeNewPostsStatusQuery>()
            .Map(dest => dest, src => src);

        config.NewConfig<GetHomeNewPostsStatusResult, GetHomeNewPostsStatusResponse>()
            .Map(dest => dest, src => src);
    }
}
