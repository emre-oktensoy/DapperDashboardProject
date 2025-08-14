using Microsoft.Data.SqlClient;
using System.Data;


namespace DapperDashboardProject.Context
{
    public class DapperContext
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
