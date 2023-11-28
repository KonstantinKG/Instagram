using System.Security.Cryptography;
using System.Text;

using Instagram.Application.Common.Interfaces.Authentication;

namespace Instagram.Infrastructure.Authentication;

public class JwtTokenHasher : IJwtTokenHasher
{
    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return BitConverter.ToString(hashBytes).Replace("-", "");
    }
}