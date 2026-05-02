using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interface;
using TaskManagement.Domain.Domain;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repository;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;

    public ProjectRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken)
    {
        _db.Projects.Add(project);
        await _db.SaveChangesAsync(cancellationToken);
        return project;
    }

    public Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return _db.Projects
            .Include(x => x.Members)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetForUserAsync(string userId, CancellationToken cancellationToken)
    {
        return await _db.ProjectMembers
            .Where(pm => pm.UserId == userId)
            .Select(pm => pm.Project!)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddMemberAsync(Guid projectId, string userId, CancellationToken cancellationToken)
    {
        var exists = await _db.ProjectMembers.AnyAsync(x => x.ProjectId == projectId && x.UserId == userId, cancellationToken);
        if (exists)
        {
            return;
        }

        _db.ProjectMembers.Add(new ProjectMember { ProjectId = projectId, UserId = userId });
        await _db.SaveChangesAsync(cancellationToken);
    }
}
