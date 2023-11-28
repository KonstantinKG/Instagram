using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenValidator
{
    public ClaimsPrincipal? ValidateToken(string token, out SecurityToken? validatedToken);
}