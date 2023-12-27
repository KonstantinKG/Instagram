using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Commands.EditPost;

public class EditPostCommandPipeline : BasePipeline<EditPostCommand, EditPostResult>
{
    public EditPostCommandPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}