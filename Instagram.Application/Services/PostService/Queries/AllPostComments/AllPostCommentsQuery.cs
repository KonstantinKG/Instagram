namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public record AllPostCommentsQuery(
    Guid PostId,
    int Page
    );