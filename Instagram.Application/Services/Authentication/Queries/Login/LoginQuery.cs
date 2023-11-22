namespace Instagram.Application.Services.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password);