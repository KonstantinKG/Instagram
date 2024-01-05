namespace Instagram.Contracts.Post._Common;

public record PostCommentResponse(
    Guid Id,
    string Content,
    PostCommentUserResponse UserResponse
);
    