using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.GetUserPostsSlider;

public record GetUserPostsSliderResponse(
    Guid? Previous,
    Guid? Next,
    PostWithCountersResponse Post
    );