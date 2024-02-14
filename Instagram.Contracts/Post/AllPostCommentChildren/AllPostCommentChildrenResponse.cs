using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostCommentChildren;

public record AllPostCommentChildrenResponse(
    long current,
    long total,
    List<PostCommentResponse> comments
);