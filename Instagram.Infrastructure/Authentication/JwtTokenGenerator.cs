using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.UserAggregate;
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
    
    public string GenerateToken(User user)
    {
        var signinSecurityKey = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256
        );
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
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