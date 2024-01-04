using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.AllPostComments;

public record AllPostCommentsResponse(
    long Current,
    long Total,
    List<AllPostCommentsComment> Comments
);

public record AllPostCommentsComment(
    Guid Id,
    string Content,
    PostCommentUser User,
    List<AllPostCommentsComment> Comments
);