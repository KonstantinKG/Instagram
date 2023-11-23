using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.UserService.Commands;

public record EditUserCommand(
    string UserId,
    string Username,
    string Fullname,
    string Email,
    string Password,
    string? Phone,
    IAppFileProxy? Image,
    string? Bio
    );