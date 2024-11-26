using Application.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Configurations.Database
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SQLiteConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqliteConnection(connectionString);
        }
    }
}
