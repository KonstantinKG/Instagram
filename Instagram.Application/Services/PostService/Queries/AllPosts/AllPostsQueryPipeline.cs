using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public class AllPostsQueryPipeline : BasePipeline<AllPostsQuery, AllResult<Post>>
{
    public AllPostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}