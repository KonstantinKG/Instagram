namespace Instagram.Infrastructure.Persistence.Connections;

public class DbConnections
{
    public const string SectionName = "ConnectionStrings";
    public string Postgres { get; set; } = null!;
}