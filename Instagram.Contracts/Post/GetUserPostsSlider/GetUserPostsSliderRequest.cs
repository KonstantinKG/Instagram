namespace Instagram.Contracts.Post.GetUserPostsSlider;

public record GetUserPostsSliderRequest(
    int Page,
    DateTime Date,
    Guid? UserId,
    Guid PostId
    );