using System.Data;
using Instagram.Infrastructure.Persistence.Connections;
using Npgsql;

namespace Instagram.Infrastructure.Persistence.Dapper.Common;

public class DapperContext
{
    private readonly DbConnections _dbConnections;

    public DapperContext(DbConnections dbConnections)
    {
        _dbConnections = dbConnections;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_dbConnections.Postgres);
    }
}