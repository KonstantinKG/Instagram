namespace Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;

public record AllPostCommentChildrenQuery(
    Guid CommentId,
    int Page
    );