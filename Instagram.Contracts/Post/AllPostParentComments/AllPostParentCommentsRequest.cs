namespace Instagram.Contracts.Post.AllPostParentComments;

public record AllPostParentCommentsRequest(
    Guid PostId,
    int Page
    );