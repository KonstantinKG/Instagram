using Instagram.Domain.Aggregates.PostAggregate;

namespace Instagram.Application.Services.PostService.Queries.GetPost;

public record GetPostResult(
    Post Post
    );