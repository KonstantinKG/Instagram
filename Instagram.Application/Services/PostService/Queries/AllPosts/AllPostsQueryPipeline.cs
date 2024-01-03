using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public class AllPostsQueryPipeline : BasePipeline<AllPostsQuery, AllPostsResult>
{
    public AllPostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}