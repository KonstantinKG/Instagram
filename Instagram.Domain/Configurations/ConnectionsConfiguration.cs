namespace Instagram.Domain.Configurations;

public class ConnectionsConfiguration
{
    public PostgresConnection Postgres { get; set; } = null!;
    public RedisConnection Redis { get; set; } = null!;
    public MinioConnection Minio { get; set; } = null!;
}

public class PostgresConnection
{
    public string Url { get; set; } = null!;
    public int BatchSize { get; set; }
}

public class RedisConnection
{
    public string Url { get; set; } = null!;
    public int AuthDatabase { get; set; }
}

public class MinioConnection
{
    public string Url { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}