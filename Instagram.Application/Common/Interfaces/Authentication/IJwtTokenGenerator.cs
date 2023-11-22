using Instagram.Domain.Entities;

namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateToken(User user);
}