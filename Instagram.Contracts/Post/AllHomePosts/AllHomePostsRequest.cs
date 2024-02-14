namespace Instagram.Contracts.Post.AllHomePosts;

public record AllHomePostsRequest(
    int page,
    DateTime date
    );