using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public record GetHomePostsSliderResult(
    Guid? Previous,
    Guid? Next,
    Post Post
    );