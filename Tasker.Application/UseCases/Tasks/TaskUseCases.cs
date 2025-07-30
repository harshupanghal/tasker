using Tasker.Application.DTOs;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities; 

namespace Tasker.Application.UseCases.Tasks;

public class CreateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    public CreateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<TaskResponse> ExecuteAsync(int userId, CreateTaskRequest request)
    {
        var task = new Domain.Entities.Task
        {
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false, 
            UserId = userId,
            CreatedAt = DateTime.UtcNow 
        };

        var newTaskId = await _taskRepository.AddTaskAsync(task);
        task.Id = newTaskId; 
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
public class GetTasksForUserUseCase
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksForUserUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<IEnumerable<TaskResponse>> ExecuteAsync(int userId)
    {
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        return tasks.Select(task => new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        }).ToList();
    }
}

public class GetTaskByIdUseCase
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponse?> ExecuteAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId, userId);

        if (task == null)
        {
            return null; 
        }

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            UserId = task.UserId,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}

public class UpdateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<bool> ExecuteAsync(int taskId, int userId, UpdateTaskRequest request)
    {
        var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
        if (existingTask == null)
        {
            return false; 
        }
        existingTask.Title = request.Title;
        existingTask.Description = request.Description;
        existingTask.IsCompleted = request.IsCompleted;
        existingTask.UpdatedAt = DateTime.UtcNow; 
        await _taskRepository.UpdateTaskAsync(existingTask);
        return true;
    }
}
public class DeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    public async Task<bool> ExecuteAsync(int taskId, int userId)
    {
        var success = await _taskRepository.DeleteTaskAsync(taskId, userId);
        return success;
    }
}
