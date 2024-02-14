namespace Instagram.Contracts.Post.UpdatePostComment;

public record UpdatePostCommentRequest(
    Guid id,
    string content
    );