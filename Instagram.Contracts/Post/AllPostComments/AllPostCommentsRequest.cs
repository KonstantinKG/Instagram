namespace Instagram.Contracts.Post.AllPostComments;

public record AllPostCommentsRequest(
    Guid post_id,
    int page
    );