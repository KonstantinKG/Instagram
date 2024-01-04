using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;

public record AllPostCommentChildrenResult(
    long Current,
    long Total,
    List<PostComment> Comments
    );