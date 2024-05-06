using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public class AllHomePostsQueryPipeline : BasePipeline<AllHomePostsQuery, AllResult<Post>>
{
    public AllHomePostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}