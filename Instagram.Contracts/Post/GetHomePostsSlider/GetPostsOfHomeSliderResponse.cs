using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.GetHomePostsSlider;

public record GetHomePostsSliderResponse(
    Guid? Previous,
    Guid? Next,
    PostShortResponse Post
    );