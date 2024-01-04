namespace Instagram.Contracts.Post.Common;

public record PostCommentResponse(
    Guid Id,
    string Content,
    PostCommentUser User
);
    