﻿using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.GetUserPostsSlider;

public record GetUserPostsSliderResponse(
    Guid? Previous,
    Guid? Next,
    PostShortResponse Post
    );