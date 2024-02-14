using Instagram.Infrastructure.Persistence.Common;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace Instagram.Infrastructure.Persistence.Redis;

public class RedisContext
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    
    public RedisContext(IOptions<DbConnections> options)
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options.Value.Redis.Connection));
    }

    public ConnectionMultiplexer Connection => _lazyConnection.Value;
}