using Instagram.Domain.Aggregates.TokenAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;

public interface IJwtTokenRepository
{
    Task<Token?> GetToken(string tokenHash);
    Task InsertToken(long userId, string sessionId, string tokenHash);
    Task DeleteToken(string tokenHash);
    Task DeleteAllUserTokens(long userId);
    Task DeleteAllSessionTokens(string sessionId);
}