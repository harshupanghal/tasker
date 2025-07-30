using Dapper;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Infrastructure.Database; 

namespace Tasker.Infrastructure.Repositories
{

    public class TaskRepository : ITaskRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public TaskRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async System.Threading.Tasks.Task<int> AddTaskAsync(Tasker.Domain.Entities.Task task)
        {
            var sql = @"
                INSERT INTO Tasks (Title, Description, IsCompleted, UserId, CreatedAt, UpdatedAt)
                VALUES (@Title, @Description, @IsCompleted, @UserId, @CreatedAt, @UpdatedAt);
                SELECT SCOPE_IDENTITY();";
            using (var connection = _connectionFactory.CreateConnection())
            {
               
                var id = await connection.ExecuteScalarAsync<int>(sql, task);
                return id;
            }
        }

        public async System.Threading.Tasks.Task<IEnumerable<Tasker.Domain.Entities.Task>> GetTasksByUserIdAsync(int userId)
        {
            var sql = "SELECT Id, Title, Description, IsCompleted, UserId, CreatedAt, UpdatedAt FROM Tasks WHERE UserId = @UserId;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Tasker.Domain.Entities.Task>(sql, new { UserId = userId });
            }
        }

        public async System.Threading.Tasks.Task<Tasker.Domain.Entities.Task?> GetTaskByIdAsync(int taskId, int userId)
        {
            var sql = "SELECT Id, Title, Description, IsCompleted, UserId, CreatedAt, UpdatedAt FROM Tasks WHERE Id = @Id AND UserId = @UserId;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Tasker.Domain.Entities.Task>(sql, new { Id = taskId, UserId = userId });
            }
        }

        public async System.Threading.Tasks.Task<bool> UpdateTaskAsync(Tasker.Domain.Entities.Task task)
        {
            var sql = @"
                UPDATE Tasks
                SET
                    Title = @Title,
                    Description = @Description,            
                    IsCompleted = @IsCompleted,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id AND UserId = @UserId;"; 
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, task);
                return rowsAffected > 0;
            }
        }

        public async System.Threading.Tasks.Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            var sql = "DELETE FROM Tasks WHERE Id = @Id AND UserId = @UserId;"; 
            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = taskId, UserId = userId });
                return rowsAffected > 0;
            }
        }
    }
}
