namespace Instagram.Contracts.Authentication;

public record AuthenticationResponse(
    string AccessToken,
    string RefreshToken
    );