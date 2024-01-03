using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public class AllUserPostsQueryPipeline : BasePipeline<AllUserPostsQuery, AllUserPostsResult>
{
    public AllUserPostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}