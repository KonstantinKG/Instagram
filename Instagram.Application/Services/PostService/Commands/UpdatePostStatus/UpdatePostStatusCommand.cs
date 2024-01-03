namespace Instagram.Application.Services.PostService.Commands.UpdatePostStatus;

public record UpdatePostStatusCommand(
    Guid Id,
    Guid UserId
);
    