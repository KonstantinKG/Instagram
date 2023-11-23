namespace Instagram.Application.Services.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Fullname,
    string Email,
    string Password
    );