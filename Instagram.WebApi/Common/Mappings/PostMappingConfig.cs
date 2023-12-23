using Instagram.Application.Services.PostService.Commands;
using Instagram.Contracts.Post.CreatePostContracts;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class PostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostRequest, CreatePostCommand>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<CreatePostRequestImage, CreatePostCommandImage>()
            .Map(dest => dest.Image , src => new AppFileProxy(src.Image))
            .Map(dest => dest, src => src);
        
        config.NewConfig<CreatePostResult, CreatePostResponse>()
            .Map(dest => dest, src => src);
    }
}