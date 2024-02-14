namespace Instagram.Contracts.Post.GetUserPostsSlider;

public record GetUserPostsSliderRequest(
    int page,
    DateTime date,
    Guid? user_id,
    Guid post_id
    );