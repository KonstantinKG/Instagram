using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;

public class AllPostCommentChildrenQueryPipeline : BasePipeline<AllPostCommentChildrenQuery, AllResult<PostComment>>
{
    public AllPostCommentChildrenQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}