namespace Instagram.Application.Services.PostService.Commands.ConfirmPost;

public record ConfirmPostCommand(
    Guid Id,
    Guid UserId
);
    