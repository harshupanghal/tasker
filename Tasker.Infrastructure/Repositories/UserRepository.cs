using Dapper;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Infrastructure.Database; 

namespace Tasker.Infrastructure.Repositories
{
   
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async System.Threading.Tasks.Task<int> AddUserAsync(User user)
        {
            var sql = "INSERT INTO Users (Username, Password, CreatedAt) VALUES (@Username, @Password, @CreatedAt); SELECT SCOPE_IDENTITY();";
            using (var connection = _connectionFactory.CreateConnection())
            { 
                var id = await connection.ExecuteScalarAsync<int>(sql, user);
                return id;
            }
        }
        public async System.Threading.Tasks.Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Username = @Username;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            }
        }

        public async System.Threading.Tasks.Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Id = @Id;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            }
        }
    }
}
