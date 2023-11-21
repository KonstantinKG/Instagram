namespace Instagram.Application.Services.Authentication;

public record LoginCommand(
    string Email,
    string Password);