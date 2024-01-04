using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public record AllPostCommentsResult(
    long Current,
    long Total,
    List<PostComment> Comments
    );