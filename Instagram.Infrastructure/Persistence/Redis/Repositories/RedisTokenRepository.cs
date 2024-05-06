using Instagram.Application.Common.Interfaces.Persistence.RedisRepositories;
using Instagram.Domain.Configurations;
using Instagram.Domain.Redis;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Instagram.Infrastructure.Persistence.Redis.Repositories;

public class RedisTokenRepository : IRedisTokenRepository
{
    private readonly RedisContext _context;
    private readonly AppConfiguration _configuration;

    public RedisTokenRepository(RedisContext context, IOptions<AppConfiguration> options)
    {
        _context = context;
        _configuration = options.Value;
    }
    
    public async Task<TokenPair?> GetSessionTokens(string sessionId)
    { 
        var db = _context.Connection.GetDatabase(_configuration.Connections.Redis.AuthDatabase);
        var serializedTokenPair = await db.StringGetAsync(sessionId);
        var pair = JsonConvert.DeserializeObject<TokenPair>(serializedTokenPair.ToString());
        return pair;
    }
}