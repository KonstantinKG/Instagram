using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostParentComments;

public record AllPostParentCommentsResult(
    long Current,
    long Total,
    List<PostComment> Comments
    );