using Instagram.Domain.Aggregates.TokenAggregate;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;

public interface IJwtTokenRepository
{
    Task<Token?> GetToken(string tokenHash);
    Task InsertToken(string userId, string sessionId, string tokenHash);
    Task DeleteToken(string tokenHash);
    Task DeleteAllUserTokens(string userId);
    Task DeleteAllSessionTokens(string sessionId);
}