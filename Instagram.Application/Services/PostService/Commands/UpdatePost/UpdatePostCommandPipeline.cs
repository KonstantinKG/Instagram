using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public class UpdatePostCommandPipeline : BasePipeline<UpdatePostCommand, bool>
{
    public UpdatePostCommandPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}