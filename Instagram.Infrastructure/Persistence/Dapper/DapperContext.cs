using System.Data;

using Instagram.Infrastructure.Persistence.Common;

using Microsoft.Extensions.Options;

using Npgsql;

namespace Instagram.Infrastructure.Persistence.Dapper;

public class DapperContext
{
    private readonly DbConnections _dbConnections;

    public DapperContext(IOptions<DbConnections> options)
    {
        _dbConnections = options.Value;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_dbConnections.Postgres);
    }
}