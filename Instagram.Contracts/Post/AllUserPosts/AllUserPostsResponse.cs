using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.AllUserPosts;

public record AllUserPostsResponse(
    long Current,
    long Total,
    List<PostResponse> Posts
);