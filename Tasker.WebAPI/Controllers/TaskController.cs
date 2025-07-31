using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs;
using Tasker.Application.UseCases.Tasks;

namespace Tasker.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // Base route: /api/Tasks
public class TasksController : ControllerBase
{
    private readonly CreateTaskUseCase _createTaskUseCase;
    private readonly GetTasksForUserUseCase _getTasksForUserUseCase;
    private readonly GetTaskByIdUseCase _getTaskByIdUseCase;
    private readonly UpdateTaskUseCase _updateTaskUseCase;
    private readonly DeleteTaskUseCase _deleteTaskUseCase;

    public TasksController(
        CreateTaskUseCase createTaskUseCase,
        GetTasksForUserUseCase getTasksForUserUseCase,
        GetTaskByIdUseCase getTaskByIdUseCase,
        UpdateTaskUseCase updateTaskUseCase,
        DeleteTaskUseCase deleteTaskUseCase)
    {
        _createTaskUseCase = createTaskUseCase;
        _getTasksForUserUseCase = getTasksForUserUseCase;
        _getTaskByIdUseCase = getTaskByIdUseCase;
        _updateTaskUseCase = updateTaskUseCase;
        _deleteTaskUseCase = deleteTaskUseCase;
    }


    // POST /api/Tasks
    [HttpPost]
    public async System.Threading.Tasks.Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, [FromHeader(Name = "X-UserId")] int userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (userId <= 0)
        {
            return Unauthorized("User ID is required for task creation.");
        }

        var response = await _createTaskUseCase.ExecuteAsync(userId, request);
        return CreatedAtAction(nameof(GetTaskById), new { id = response.Id, userId = userId }, response);
    }

    // GET /api/Tasks?userId={userId}
    [HttpGet]
    public async System.Threading.Tasks.Task<IActionResult> GetTasksForUser([FromQuery] int userId)
    {
        if (userId <= 0)
        {
            return BadRequest("User ID is required to retrieve tasks.");
        }

        var tasks = await _getTasksForUserUseCase.ExecuteAsync(userId);
        return Ok(tasks);
    }

    // GET /api/Tasks/{id}?userId={userId}
    [HttpGet("{id}")]
    public async System.Threading.Tasks.Task<IActionResult> GetTaskById(int id, [FromQuery] int userId)
    {
        if (userId <= 0)
        {
            return BadRequest("User ID is required to retrieve a specific task.");
        }

        var task = await _getTaskByIdUseCase.ExecuteAsync(id, userId);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    // PUT /api/Tasks/{id}
   
    [HttpPut("{id}")]
    public async System.Threading.Tasks.Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request, [FromHeader(Name = "X-UserId")] int userId)
    {
        if (id != request.Id)
        {
            return BadRequest("Task ID in URL must match ID in body.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (userId <= 0)
        {
            return Unauthorized("User ID is required for task update.");
        }

        var success = await _updateTaskUseCase.ExecuteAsync(id, userId, request);
        if (!success)
        {
            return NotFound("Task not found or not owned by the specified user.");
        }
        return NoContent(); 
    }

    // DELETE /api/Tasks/{id}?userId={userId}
    [HttpDelete("{id}")]
    public async System.Threading.Tasks.Task<IActionResult> DeleteTask(int id, [FromQuery] int userId)
    {
        if (userId <= 0)
        {
            return Unauthorized("User ID is required for task deletion.");
        }

        var success = await _deleteTaskUseCase.ExecuteAsync(id, userId);
        if (!success)
        {
            return NotFound("Task not found or not owned by the specified user.");
        }
        return NoContent(); 
    }
}
