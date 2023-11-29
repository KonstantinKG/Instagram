using Microsoft.IdentityModel.Tokens;

namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateAccessToken(TokenParameters parameters);
    public string GenerateRefreshToken(TokenParameters parameters);
    public string RotateRefreshToken(TokenParameters parameters, SecurityToken validatedToken);
}

public record TokenParameters
(
    string Id,
    string SessionId,
    string Username,
    string Email
);