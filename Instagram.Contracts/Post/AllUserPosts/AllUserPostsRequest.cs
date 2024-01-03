namespace Instagram.Contracts.Post.AllUserPosts;

public record AllUserPostsRequest(
    int Page,
    DateTime Date,
    Guid? UserId
    );