using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.UserService.Commands.EditUser;

public record EditUserCommand(
    Guid Id,
    string Username,
    string Fullname,
    string Email,
    string? Phone,
    IAppFileProxy? Image,
    string? Bio,
    string? Gender
    );