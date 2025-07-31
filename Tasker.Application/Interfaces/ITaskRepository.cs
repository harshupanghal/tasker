using Tasker.Domain.Entities; 

// interface for task related functions

namespace Tasker.Application.Interfaces;
public interface ITaskRepository 
{
    public Task<int> AddTaskAsync(Tasker.Domain.Entities.Task task); 
    public Task<IEnumerable<Tasker.Domain.Entities.Task>> GetTasksByUserIdAsync(int userId); 
    public Task<Tasker.Domain.Entities.Task?> GetTaskByIdAsync(int taskId, int userId);
    public Task<bool> UpdateTaskAsync(Tasker.Domain.Entities.Task task);
    public Task<bool> DeleteTaskAsync(int taskId, int userId);
}
