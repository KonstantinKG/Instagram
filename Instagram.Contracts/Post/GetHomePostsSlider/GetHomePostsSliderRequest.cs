namespace Instagram.Contracts.Post.GetHomePostsSlider;

public record GetHomePostsSliderRequest(
    int page,
    DateTime date,
    Guid post_id
    );