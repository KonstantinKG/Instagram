namespace Instagram.Infrastructure.Persistence.Common;

public class DbConnections
{
    public const string SectionName = "Connections";
    public string Postgres { get; set; } = null!;
    public RedisConnection Redis { get; set; } = null!;
}

public class RedisConnection
{
    public string Connection { get; set; } = null!;
    public short AuthDatabase { get; set; } 
}