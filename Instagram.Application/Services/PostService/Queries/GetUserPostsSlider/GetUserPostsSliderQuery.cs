namespace Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;

public record GetUserPostsSliderQuery(
    int Page,
    DateTime Date,
    Guid UserId,
    Guid PostId
);