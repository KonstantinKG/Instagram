namespace Instagram.Contracts.Authentication;

public record AuthenticationResponse(
    long Id,
    string Username,
    string Fullname,
    string Email,
    string? Phone,
    string Token
    );