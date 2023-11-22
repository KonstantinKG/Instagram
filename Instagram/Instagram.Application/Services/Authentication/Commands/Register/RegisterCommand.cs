namespace Instagram.Application.Services.Authentication.Commands.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password
    );