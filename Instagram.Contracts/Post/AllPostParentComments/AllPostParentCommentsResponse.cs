using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostParentComments;

public record AllPostParentCommentsResponse(
    long Current,
    long Total,
    List<PostCommentResponse> Comments
);