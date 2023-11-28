using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Instagram.Application.Common.Interfaces.Authentication;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Infrastructure.Authentication;

public class JwtTokenValidator : IJwtTokenValidator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenValidator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    
    public ClaimsPrincipal? ValidateToken(string token , out SecurityToken? validatedToken)
    {
        var securityKey = new RsaSecurityKey(RsaKeyGenerator.RsaKey);
        
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = securityKey
        };

        try
        {
            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            jwtSecurityHandler.MapInboundClaims = false;
            return jwtSecurityHandler.ValidateToken(
                token,
                validationParameters,
                out validatedToken
            );
        }
        catch (Exception)
        {
            validatedToken = null;
            return null;
        }
    }
}