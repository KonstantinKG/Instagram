namespace Instagram.Application.Services.UserService.Commands.UnsubscribeUser;

public record UnsubscribeUserCommand(
    Guid SubscriberId,
    Guid UserId);