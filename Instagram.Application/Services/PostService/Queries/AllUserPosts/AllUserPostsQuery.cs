namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public record AllUserPostsQuery(
    int Page,
    DateTime Date,
    Guid UserId
    );