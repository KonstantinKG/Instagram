using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPosts;

public record AllPostsResponse(
    long current,
    long total,
    List<PostResponse> posts
);