using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostCommentChildren;

public record AllPostCommentChildrenResponse(
    long Current,
    long Total,
    List<PostCommentResponse> Comments
);