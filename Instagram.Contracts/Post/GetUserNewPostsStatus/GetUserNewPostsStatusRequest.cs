namespace Instagram.Contracts.Post.GetUserNewPostsStatus;

public record GetUserNewPostsStatusRequest(
    Guid UserId,
    DateTime Date
    );