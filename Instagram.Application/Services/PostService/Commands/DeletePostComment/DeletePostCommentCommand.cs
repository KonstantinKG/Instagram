namespace Instagram.Application.Services.PostService.Commands.DeletePostComment;

public record DeletePostCommentCommand(
    Guid Id,
    Guid UserId
);
    