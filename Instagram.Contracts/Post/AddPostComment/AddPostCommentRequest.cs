namespace Instagram.Contracts.Post.AddPostComment;

public record AddPostCommentRequest(
    Guid PostId,
    Guid? ParentId,
    string Content
    );