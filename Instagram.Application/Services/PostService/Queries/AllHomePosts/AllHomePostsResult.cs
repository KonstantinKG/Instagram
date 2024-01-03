using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public record AllHomePostsResult(
    long Current,
    long Total,
    List<Post> Posts
    );