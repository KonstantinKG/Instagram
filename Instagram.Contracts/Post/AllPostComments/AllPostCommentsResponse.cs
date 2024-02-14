using Instagram.Contracts.Post._Common;

namespace Instagram.Contracts.Post.AllPostComments;

public record AllPostCommentsResponse(
    long current,
    long total,
    List<PostCommentResponse> comments
);