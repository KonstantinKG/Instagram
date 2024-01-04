using Instagram.Contracts.Post.Common;

namespace Instagram.Contracts.Post.AllPostParentComments;

public record AllPostParentCommentsResponse(
    long Current,
    long Total,
    List<PostCommentResponse> Comments
);