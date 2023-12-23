using Instagram.Domain.Aggregates.TokenAggregate;

namespace Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;

public interface IJwtTokenRepository
{
    Task<Token?> GetToken(string tokenHash);
    Task InsertToken(string userId, string sessionId, string tokenHash);
    Task DeleteToken(string tokenHash);
    Task DeleteAllUserTokens(string userId);
    Task DeleteAllSessionTokens(string sessionId);
}