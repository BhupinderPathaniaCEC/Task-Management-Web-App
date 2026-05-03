using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Contracts.Tasks;
using TaskManagement.Application.Interface;
using TaskManagement.Domain.Domain;
using DomainTaskStatus = TaskManagement.Domain.Domain.TaskStatus;

namespace TaskManagement.Api.Controller;

[ApiController]
[Route("api/tasks")]
[Authorize]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetForProject(Guid projectId, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByProjectAsync(projectId, cancellationToken);
        var response = tasks.Select(t => new TaskResponse(t.Id, t.ProjectId, t.Title, t.Description, t.Status, t.DueDate, t.AssigneeUserId, t.CreatedAt)).ToList();
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = AppRole.Admin)]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest();
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            AssigneeUserId = request.AssigneeUserId,
            DueDate = request.DueDate,
            Status = DomainTaskStatus.Todo,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _taskRepository.AddAsync(task, cancellationToken);

        return Ok(new TaskResponse(task.Id, task.ProjectId, task.Title, task.Description, task.Status, task.DueDate, task.AssigneeUserId, task.CreatedAt));
    }

    [HttpPatch("{taskId:guid}/status")]
    [Authorize(Roles = AppRole.Admin)]
    public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return NotFound();
        }

        task.Status = request.Status;
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return NoContent();
    }
}
