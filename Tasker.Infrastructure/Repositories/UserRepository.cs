using Dapper;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Infrastructure.Database;

// all the user related operations here

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
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var id = await connection.ExecuteScalarAsync<int>(sql, user);
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not add user to the database.", ex);
            }
        }
        public async System.Threading.Tasks.Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Username = @Username;";
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve user by username.", ex);
            }
        }

        public async System.Threading.Tasks.Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT Id, Username, Password, CreatedAt FROM Users WHERE Id = @Id;";
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve user by ID.", ex);
            }
        }
    }
}
