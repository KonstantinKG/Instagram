using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.AllHomePosts;

public record AllHomePostsResponse(
    long Current,
    long Total,
    List<PostResponse> Posts
);