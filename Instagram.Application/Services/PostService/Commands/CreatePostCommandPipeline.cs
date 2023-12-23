using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Commands;

public class CreatePostCommandPipeline : BasePipeline<CreatePostCommand, CreatePostResult>
{
    public CreatePostCommandPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}