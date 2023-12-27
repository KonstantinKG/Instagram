using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetAllPosts;

public record GetAllPostsResult(
    long Current,
    long Total,
    List<Post> Posts
    );