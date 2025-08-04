using Microsoft.EntityFrameworkCore;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Infrastructure.Persistence;

namespace Tasker.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
    {
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
        {
        _context = context;
        }

    public async Task<int> AddTaskAsync(Domain.Entities.Task task)
        {
        try
            {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task.Id;
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Could not add task to the database.", ex);
            }
        }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserIdAsync(int userId)
        {
        try
            {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Could not retrieve user's tasks from the database.", ex);
            }
        }

    public async Task<Domain.Entities.Task?> GetTaskByIdAsync(int taskId, int userId)
        {
        try
            {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Could not get task from the database.", ex);
            }
        }

    public async Task<bool> UpdateTaskAsync(Domain.Entities.Task task)
        {
        try
            {
            var existingTask = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == task.Id && t.UserId == task.UserId);

            if (existingTask == null)
                return false;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.UpdatedAt = task.UpdatedAt;

            await _context.SaveChangesAsync();
            return true;
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Could not update task in the database.", ex);
            }
        }

    public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
        try
            {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
            }
        catch (Exception ex)
            {
            throw new InvalidOperationException("Could not delete task from the database.", ex);
            }
        }
    }

