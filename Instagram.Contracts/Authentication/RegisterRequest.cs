namespace Instagram.Contracts.Authentication;

public record RegisterRequest(
    string Username,
    string Fullname,
    string Email,
    string Password
);