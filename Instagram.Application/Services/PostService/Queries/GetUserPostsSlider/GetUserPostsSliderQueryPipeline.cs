using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services._Common;
using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;

public class GetUserPostsSliderQueryPipeline : BasePipeline<GetUserPostsSliderQuery, SliderResult<Post>>
{
    public GetUserPostsSliderQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}