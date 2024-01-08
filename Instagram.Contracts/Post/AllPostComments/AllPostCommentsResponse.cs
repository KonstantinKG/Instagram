using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostComments;

public record AllPostCommentsResponse(
    long Current,
    long Total,
    List<AllPostCommentsComment> Comments
);

public record AllPostCommentsComment(
    Guid Id,
    string Content,
    PostCommentUserResponse User,
    List<AllPostCommentsComment> Comments
);