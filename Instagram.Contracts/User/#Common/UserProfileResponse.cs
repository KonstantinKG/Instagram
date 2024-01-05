namespace Instagram.Contracts.User._Common;

public record UserProfileResponse(
    string? Image,
    string? Bio,
    string? Gender
);