namespace Instagram.Contracts.Post.GetHomePostsSlider;

public record GetHomePostsSliderRequest(
    int Page,
    DateTime Date,
    Guid PostId
    );