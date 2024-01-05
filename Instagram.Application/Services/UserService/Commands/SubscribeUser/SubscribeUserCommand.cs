namespace Instagram.Application.Services.UserService.Commands.SubscribeUser;

public record SubscribeUserCommand(
    Guid SubscriberId,
    Guid UserId);