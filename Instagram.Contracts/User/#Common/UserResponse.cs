namespace Instagram.Contracts.User._Common;

public record UserResponse(
    Guid id,
    string username,
    string? email,
    string? phone,
    UserProfileResponse? profile
);