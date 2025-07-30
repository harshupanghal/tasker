namespace Tasker.Application.DTOs;
public class CreateTaskRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateTaskRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}
public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
