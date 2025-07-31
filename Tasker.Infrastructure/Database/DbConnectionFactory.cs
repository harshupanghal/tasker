using Microsoft.Data.SqlClient; 
using System.Data;

// this program helps to connect our application with database.

namespace Tasker.Infrastructure.Database
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public IDbConnection CreateConnection()
        {
            try {

                var connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not create a database connection.", ex);
            }
        }
    }
}
