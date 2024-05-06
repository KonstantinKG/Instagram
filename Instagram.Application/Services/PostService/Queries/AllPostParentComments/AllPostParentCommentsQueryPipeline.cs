using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostParentComments;

public class AllPostParentCommentsQueryPipeline : BasePipeline<AllPostParentCommentsQuery, AllResult<PostComment>>
{
    public AllPostParentCommentsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}