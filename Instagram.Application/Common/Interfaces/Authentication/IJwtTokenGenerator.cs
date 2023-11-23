using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateToken(User user);
}