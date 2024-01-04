namespace Instagram.Contracts.Post.AllPostComments;

public record AllPostCommentsRequest(
    Guid PostId,
    int Page
    );