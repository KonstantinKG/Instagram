using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.AllPosts;

public record AllPostsResponse(
    long Current,
    long Total,
    List<PostResponse> Posts
);