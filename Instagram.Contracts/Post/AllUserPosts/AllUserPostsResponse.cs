using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllUserPosts;

public record AllUserPostsResponse(
    long current,
    long total,
    List<PostResponse> posts
);