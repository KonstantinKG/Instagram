using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public class AllPostCommentsQueryPipeline : BasePipeline<AllPostCommentsQuery, AllResult<PostComment>>
{
    public AllPostCommentsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}