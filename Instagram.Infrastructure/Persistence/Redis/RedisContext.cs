using Instagram.Domain.Configurations;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace Instagram.Infrastructure.Persistence.Redis;

public class RedisContext
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
    
    public RedisContext(IOptions<AppConfiguration> options)
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options.Value.Connections.Redis.Url));
    }

    public ConnectionMultiplexer Connection => _lazyConnection.Value;
}