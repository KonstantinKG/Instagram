using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly DateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(DateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }
    
    public string GenerateToken(Guid userId, string name)
    {
        var signinSecurityKey = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256
        );
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, name),
            new Claim(JwtRegisteredClaimNames.FamilyName, "empty"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpirationTimeInMinutes)),
            claims: claims,
            signingCredentials: signinSecurityKey
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}