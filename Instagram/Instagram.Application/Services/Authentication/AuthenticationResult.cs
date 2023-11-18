namespace Instagram.Application.Services.Authentication;

public record AuthenticationResult(
    Guid Id,
    string Name,
    string Email, 
    string Token
    );