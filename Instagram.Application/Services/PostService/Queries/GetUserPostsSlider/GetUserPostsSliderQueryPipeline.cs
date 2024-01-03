using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;

public class GetUserPostsSliderQueryPipeline : BasePipeline<GetUserPostsSliderQuery, GetUserPostsSliderResult>
{
    public GetUserPostsSliderQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}