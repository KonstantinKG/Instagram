using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public record AllPostsResult(
    long Current,
    long Total,
    List<Post> Posts
    );