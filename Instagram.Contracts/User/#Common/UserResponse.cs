namespace Instagram.Contracts.User._Common;

public record UserResponse(
    Guid Id,
    string Username,
    string Fullname,
    string? Email,
    string? Phone,
    UserProfileResponse Profile
);