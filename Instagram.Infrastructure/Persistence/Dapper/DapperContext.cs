using System.Data;
using Instagram.Domain.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Instagram.Infrastructure.Persistence.Dapper;

public class DapperContext
{
    private readonly AppConfiguration _configuration;

    public DapperContext(IOptions<AppConfiguration> options)
    {
        _configuration = options.Value;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_configuration.Connections.Postgres.Url);
    }
}