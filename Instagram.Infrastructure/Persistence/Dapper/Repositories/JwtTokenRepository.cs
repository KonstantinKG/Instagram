using Dapper;

using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Domain.Aggregates.TokenAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;

namespace Instagram.Infrastructure.Persistence.Dapper.Repositories;

public class JwtTokenRepository : IJwtTokenRepository
{
    private readonly DapperContext _context;

    public JwtTokenRepository(DapperContext context)
    {
        _context = context;
    }

    
    public async Task<Token?> GetToken(string tokenHash)
    {
        var connection = _context.CreateConnection();
        var parameters = new { TokenHash = tokenHash };
        const string sql = 
            """
                SELECT * FROM tokens WHERE hash = @tokenHash;
            """;
        
        var token = await connection.QuerySingleOrDefaultAsync<Token>(sql, parameters);

        return token;
    }

    public async Task InsertToken(long userId, string sessionId, string tokenHash)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId, SessionId = sessionId, TokenHash = tokenHash };
        const string sql = 
            """
                INSERT INTO tokens (user_id, session_id, hash) VALUES (@userId, @sessionId, @tokenHash);
            """;
        
        var token = await connection.ExecuteAsync(sql, parameters);
    }

    public async Task DeleteToken(string tokenHash)
    {
        var connection = _context.CreateConnection();
        var parameters = new { TokenHash = tokenHash };
        const string sql = 
            """
                DELETE FROM tokens WHERE hash = @tokenHash;
            """;
        
        var token = await connection.ExecuteAsync(sql, parameters);
    }

    public async Task DeleteAllUserTokens(long userId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { UserId = userId };
        const string sql = 
            """
                DELETE FROM tokens WHERE user_id = @userId;
            """;
        
        var token = await connection.ExecuteAsync(sql, parameters);
    }
    
    public async Task DeleteAllSessionTokens(string sessionId)
    {
        var connection = _context.CreateConnection();
        var parameters = new { SessionId = sessionId };
        const string sql = 
            """
                DELETE FROM tokens WHERE session_id = @sessionId;
            """;
        
        var token = await connection.ExecuteAsync(sql, parameters);
    }
}