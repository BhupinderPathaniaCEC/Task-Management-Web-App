using System.Security.Claims;
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
    private readonly IProjectRepository _projectRepository;

    public TasksController(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetForProject(Guid projectId, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByProjectAsync(projectId, cancellationToken);
        var response = tasks.Select(t => new TaskResponse(t.Id, t.ProjectId, t.Title, t.Description, t.Status, t.DueDate, t.AssigneeUserId, t.CreatedAt)).ToList();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Creating task: {request.Title} for project: {request.ProjectId}");
        
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required");
        }

        // Verify project exists
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            return BadRequest("Project not found");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // Use current user as assignee if none provided or invalid
        var assigneeId = request.AssigneeUserId;
        if (string.IsNullOrWhiteSpace(assigneeId) || assigneeId == "1")
        {
            assigneeId = currentUserId;
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            AssigneeUserId = assigneeId,
            DueDate = request.DueDate,
            Status = DomainTaskStatus.Todo,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _taskRepository.AddAsync(task, cancellationToken);
        Console.WriteLine($"Task created successfully: {task.Id}");

        return Ok(new TaskResponse(task.Id, task.ProjectId, task.Title, task.Description, task.Status, task.DueDate, task.AssigneeUserId, task.CreatedAt));
    }

    [HttpPatch("{taskId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole(AppRole.Admin);
        
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return NotFound();
        }

        // Allow admin OR assigned user to update status
        if (!isAdmin && task.AssigneeUserId != userId)
        {
            return Forbid();
        }

        task.Status = request.Status;
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return NoContent();
    }
}
