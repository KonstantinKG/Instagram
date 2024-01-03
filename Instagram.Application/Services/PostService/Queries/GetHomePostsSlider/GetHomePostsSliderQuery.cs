namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public record GetHomePostsSliderQuery(
    int Page,
    DateTime Date,
    Guid PostId
);