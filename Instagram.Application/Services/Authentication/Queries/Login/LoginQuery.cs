namespace Instagram.Application.Services.Authentication.Queries.Login;

public record LoginQuery(
    string Identity,
    string Password);