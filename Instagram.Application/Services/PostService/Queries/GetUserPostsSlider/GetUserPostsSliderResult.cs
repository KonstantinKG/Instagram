using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;

public record GetUserPostsSliderResult(
    Guid? Previous,
    Guid? Next,
    Post Post
    );