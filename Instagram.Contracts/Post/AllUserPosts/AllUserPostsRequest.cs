namespace Instagram.Contracts.Post.AllUserPosts;

public record AllUserPostsRequest(
    int page,
    DateTime date,
    Guid? user_id
    );