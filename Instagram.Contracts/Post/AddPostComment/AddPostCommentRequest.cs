namespace Instagram.Contracts.Post.AddPostComment;

public record AddPostCommentRequest(
    Guid post_id,
    Guid? parent_id,
    string content
    );