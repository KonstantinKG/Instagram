using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.UserService.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string Username,
    string Fullname,
    string Email,
    string? Phone,
    IAppFileProxy? Image,
    string? Bio,
    string? Gender);