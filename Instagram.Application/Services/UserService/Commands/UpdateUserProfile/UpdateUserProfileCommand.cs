using Instagram.Application.Common.Interfaces.Services;

namespace Instagram.Application.Services.UserService.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    Guid UserId,
    string Fullname,
    IAppFileProxy? Image,
    string? Bio,
    string? Gender
    );