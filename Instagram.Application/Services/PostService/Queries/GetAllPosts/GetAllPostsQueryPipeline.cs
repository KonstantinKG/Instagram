using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.GetAllPosts;

public class GetAllPostsQueryPipeline : BasePipeline<GetAllPostsQuery, GetAllPostsResult>
{
    public GetAllPostsQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}