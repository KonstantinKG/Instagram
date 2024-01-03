using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public class AllHomePostsQueryPipeline : BasePipeline<AllHomePostsQuery, AllHomePostsResult>
{
    public AllHomePostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}