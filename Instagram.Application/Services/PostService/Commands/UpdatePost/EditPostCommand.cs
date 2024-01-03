namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public record EditPostCommand(
    Guid Id,
    Guid UserId,
    string? Content,
    long? LocationId,
    bool HideStats,
    bool HideComments
);
    