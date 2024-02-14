namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public record UpdatePostCommand(
    Guid Id,
    Guid UserId,
    string? Content,
    long? LocationId,
    bool HideStats,
    bool HideComments,
    List<string> Tags
);
    