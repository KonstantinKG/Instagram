namespace Instagram.Contracts.Post.UpdatePostComment;

public record UpdatePostCommentRequest(
    Guid Id,
    string Content
    );