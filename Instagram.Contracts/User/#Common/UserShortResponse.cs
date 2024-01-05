namespace Instagram.Contracts.User._Common;

public record UserShortResponse(
    Guid Id,
    string Username,
    string Fullname,
    string? Image
);