namespace Instagram.Contracts.Post.AllPostCommentChildren;

public record AllPostCommentChildrenRequest(
    Guid comment_id,
    int page
    );