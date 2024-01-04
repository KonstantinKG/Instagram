using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllPostParentComments;

public class AllPostParentCommentsQueryPipeline : BasePipeline<AllPostParentCommentsQuery, AllPostParentCommentsResult>
{
    public AllPostParentCommentsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}