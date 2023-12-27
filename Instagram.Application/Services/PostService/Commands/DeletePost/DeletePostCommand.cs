namespace Instagram.Application.Services.PostService.Commands.DeletePost;

public record DeletePostCommand(
    Guid Id,
    Guid UserId
);
    