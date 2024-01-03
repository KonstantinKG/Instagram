using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.GetHomePostsSlider;

public record GetHomePostsSliderResponse(
    Guid? Previous,
    Guid? Next,
    PostWithCountersResponse Post
    );