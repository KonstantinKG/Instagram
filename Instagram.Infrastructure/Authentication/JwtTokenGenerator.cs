using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateAccessToken(TokenParameters parameters)
    {
        var expiryMinutes = Convert.ToDouble(_jwtSettings.AccessTokenExpiryMinutes); 
        var validTo = _dateTimeProvider.UtcNow.AddMinutes(expiryMinutes);
        return GenerateToken(parameters, validTo);
    }
    
    public string GenerateRefreshToken(TokenParameters parameters)
    {
        var expiryMinutes = Convert.ToDouble(_jwtSettings.RefreshTokenExpiryMinutes);
        var validTo = _dateTimeProvider.UtcNow.AddMinutes(expiryMinutes);
        return GenerateToken(parameters, validTo);
    }


    public string RotateRefreshToken(TokenParameters parameters, SecurityToken validatedToken)
    {
        return GenerateToken(parameters, validatedToken.ValidTo);
    }

    private string GenerateToken(TokenParameters parameters, DateTime validTo)
    {
        var securityKey = new RsaSecurityKey(RsaKeyGenerator.RsaKey);

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.RsaSha256
        );
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, parameters.SessionId),
            new Claim(JwtRegisteredClaimNames.NameId, parameters.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, parameters.Username),
            new Claim(JwtRegisteredClaimNames.Email, parameters.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: validTo,
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}