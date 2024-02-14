namespace Instagram.Contracts.User._Common;

public record UserProfileResponse(
    string? image,
    string? bio,
    string? full_name,
    string? gender
);