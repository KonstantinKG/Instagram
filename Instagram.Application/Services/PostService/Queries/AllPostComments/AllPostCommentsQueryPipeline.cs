using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public class AllPostCommentsQueryPipeline : BasePipeline<AllPostCommentsQuery, AllPostCommentsResult>
{
    public AllPostCommentsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}