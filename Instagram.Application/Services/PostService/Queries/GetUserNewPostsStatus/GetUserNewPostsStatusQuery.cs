namespace Instagram.Application.Services.PostService.Queries.GetUserNewPostsStatus;

public record GetUserNewPostsStatusQuery(
    Guid UserId,
    DateTime Date
);