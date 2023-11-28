using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateAccessToken(TokenParameters parameters);
    public string GenerateRefreshToken(TokenParameters parameters);
    public string GenerateToken(TokenParameters parameters, DateTime validTo);
}

public record TokenParameters
(
    string Id,
    string SessionId,
    string Username,
    string Email
);