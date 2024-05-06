using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services._Common;
using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public class GetHomePostsSliderQueryPipeline : BasePipeline<GetHomePostsSliderQuery, SliderResult<Post>>
{
    public GetHomePostsSliderQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}