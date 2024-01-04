namespace Instagram.Contracts.Post.AllPostCommentChildren;

public record AllPostCommentChildrenRequest(
    Guid CommentId,
    int Page
    );