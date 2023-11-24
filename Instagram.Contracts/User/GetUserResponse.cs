using Instagram.Contracts.User.Common;

namespace Instagram.Contracts.User;

public record GetUserResponse(
    string Username,
    string Fullname,
    string? Email,
    string? Phone,
    UserProfileResponse Profile
    );