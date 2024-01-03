namespace Instagram.Application.Services.PostService.Commands.UpdatePostComment;

public record UpdatePostCommentCommand(
    Guid Id,
    Guid UserId,
    string Content
);
    