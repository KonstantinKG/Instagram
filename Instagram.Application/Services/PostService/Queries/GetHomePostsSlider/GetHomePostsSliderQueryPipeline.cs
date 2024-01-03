using Instagram.Application.Common.Interfaces.Pipeline;

namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public class GetHomePostsSliderQueryPipeline : BasePipeline<GetHomePostsSliderQuery, GetHomePostsSliderResult>
{
    public GetHomePostsSliderQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}