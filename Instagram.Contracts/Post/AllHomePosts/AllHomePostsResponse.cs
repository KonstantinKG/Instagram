using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllHomePosts;

public record AllHomePostsResponse(
    long current,
    long total,
    List<PostResponse> posts
);