namespace Instagram.Contracts.Post.AllPostParentComments;

public record AllPostParentCommentsRequest(
    Guid post_id,
    int page
    );