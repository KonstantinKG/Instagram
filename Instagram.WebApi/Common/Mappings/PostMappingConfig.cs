using Instagram.Application.Services.PostService.Commands.AddPostGallery;
using Instagram.Application.Services.PostService.Commands.ConfirmPost;
using Instagram.Application.Services.PostService.Commands.CreatePost;
using Instagram.Application.Services.PostService.Commands.DeletePost;
using Instagram.Application.Services.PostService.Commands.EditPost;
using Instagram.Application.Services.PostService.Commands.EditPostGallery;
using Instagram.Application.Services.PostService.Queries.GetAllPosts;
using Instagram.Application.Services.PostService.Queries.GetPost;
using Instagram.Contracts.Post.AddPostGalleryContracts;
using Instagram.Contracts.Post.ConfirmPostContracts;
using Instagram.Contracts.Post.CreatePostContracts;
using Instagram.Contracts.Post.DeletePostContracts;
using Instagram.Contracts.Post.EditPostContracts;
using Instagram.Contracts.Post.EditPostGalleryContracts;
using Instagram.Contracts.Post.GetAllPostsContracts;
using Instagram.Contracts.Post.GetPostContracts;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class PostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostResult, CreatePostResponse>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid userId, EditPostRequest request), EditPostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<GetPostRequest, GetPostQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetPostResult, GetPostResponse>()
            .Map(dest => dest.Galleries, src => src.Post.Galleries)
            .Map(dest => dest, src => src.Post);
        
        config.NewConfig<GetAllPostsRequest, GetAllPostsQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid userId, AddPostGalleryRequest request), AddPostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.File , src => new AppFileProxy(src.request.File))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, EditPostGalleryRequest request), EditPostGalleryCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.File , src => new AppFileProxy(src.request.File))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, ConfirmPostRequest request), ConfirmPostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid userId, DeletePostRequest request), DeletePostCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
}
