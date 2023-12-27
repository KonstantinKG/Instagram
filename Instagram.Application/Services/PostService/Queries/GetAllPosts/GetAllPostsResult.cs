using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetAllPosts;

public record GetAllPostsResult(
    List<Post> Posts
    );