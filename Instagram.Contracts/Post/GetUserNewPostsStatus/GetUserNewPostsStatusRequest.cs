namespace Instagram.Contracts.Post.GetUserNewPostsStatus;

public record GetUserNewPostsStatusRequest(
    Guid user_id,
    DateTime date
    );