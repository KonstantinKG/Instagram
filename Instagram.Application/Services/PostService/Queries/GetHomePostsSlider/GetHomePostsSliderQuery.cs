namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public record GetHomePostsSliderQuery(
    Guid UserId,
    int Page,
    DateTime Date,
    Guid PostId
);