using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostParentComments;

public record AllPostParentCommentsResponse(
    long current,
    long total,
    List<PostCommentResponse> comments
);