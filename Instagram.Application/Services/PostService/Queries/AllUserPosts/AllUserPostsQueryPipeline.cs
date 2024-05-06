using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public class AllUserPostsQueryPipeline : BasePipeline<AllUserPostsQuery, AllResult<Post>>
{
    public AllUserPostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}