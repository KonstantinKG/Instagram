namespace Instagram.Application.Services.Authentication;

public record RegisterCommand(
    string Name,
    string Email,
    string Password
    );