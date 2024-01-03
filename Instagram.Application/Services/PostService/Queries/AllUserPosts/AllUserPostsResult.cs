using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public record AllUserPostsResult(
    long Current,
    long Total,
    List<Post> Posts
    );