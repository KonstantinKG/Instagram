using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllUserPosts;

public record AllUserPostsResponse(
    long Current,
    long Total,
    List<PostShortResponse> Posts
);