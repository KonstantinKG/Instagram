using Instagram.Domain.Aggregates.PostAggregate.Entities;

namespace Instagram.Application.Services.PostService.Commands.AddPostComment;

public record AddPostCommentResult(
    PostComment Comment
    );