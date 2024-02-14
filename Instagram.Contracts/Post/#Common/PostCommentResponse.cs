using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.Post._Common;

public record PostCommentResponse(
    Guid id,
    string content,
    UserResponse user,
    List<PostCommentResponse>? comments
);
    