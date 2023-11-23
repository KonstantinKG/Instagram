namespace Instagram.Application.Services.Authentication.Common;

public record AuthenticationResult(
    string AccessToken,
    string RefreshToken
    );