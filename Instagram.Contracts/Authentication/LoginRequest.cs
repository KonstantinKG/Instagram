namespace Instagram.Contracts.Authentication;

public record LoginRequest
(
    string Identity,
    string Password
    );