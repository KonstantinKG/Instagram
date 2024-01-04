namespace Instagram.Application.Services.PostService.Queries.AllPostParentComments;

public record AllPostParentCommentsQuery(
    Guid PostId,
    int Page
    );