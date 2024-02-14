namespace Instagram.Contracts.Post.AllPosts;

public record AllPostsRequest(
    int page,
    DateTime date
    );