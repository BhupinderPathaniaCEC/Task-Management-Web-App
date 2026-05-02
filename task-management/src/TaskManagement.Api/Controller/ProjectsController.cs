using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Contracts.Projects;
using TaskManagement.Application.Interface;
using TaskManagement.Domain.Domain;

namespace TaskManagement.Api.Controller;

[ApiController]
[Route("api/projects")]
[Authorize]
public sealed class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectResponse>>> GetMyProjects(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var projects = await _projectRepository.GetForUserAsync(userId, cancellationToken);
        var response = projects.Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.CreatedAt)).ToList();
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = AppRole.Admin)]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest();
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedByUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _projectRepository.AddAsync(project, cancellationToken);
        await _projectRepository.AddMemberAsync(project.Id, userId, cancellationToken);

        return Ok(new ProjectResponse(project.Id, project.Name, project.Description, project.CreatedAt));
    }

    [HttpPost("{projectId:guid}/members")]
    [Authorize(Roles = AppRole.Admin)]
    public async Task<IActionResult> AddMember(Guid projectId, AddMemberRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return BadRequest();
        }

        await _projectRepository.AddMemberAsync(projectId, request.UserId, cancellationToken);
        return NoContent();
    }
}
