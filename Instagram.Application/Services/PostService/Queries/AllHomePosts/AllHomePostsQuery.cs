namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public record AllHomePostsQuery(
    Guid UserId,
    int Page,
    DateTime Date
    );