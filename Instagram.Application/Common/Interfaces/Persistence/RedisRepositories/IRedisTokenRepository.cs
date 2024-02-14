using Instagram.Domain.Redis;

namespace Instagram.Application.Common.Interfaces.Persistence.RedisRepositories;

public interface IRedisTokenRepository
{
    Task<TokenPair?> GetSessionTokens(string sessionId);
}