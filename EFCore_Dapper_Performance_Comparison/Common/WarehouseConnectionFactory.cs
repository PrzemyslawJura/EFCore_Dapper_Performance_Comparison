using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace EFCore_Dapper_Performance_Comparison.Common.WarehouseConnectionFactory
{
    public class WarehouseConnectionFactory
    {
        private readonly string _connectionString;
        public WarehouseConnectionFactory(string connectionString)
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
