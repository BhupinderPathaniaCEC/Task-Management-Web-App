using TaskManagement.Domain.Domain;

namespace TaskManagement.Application.Interface;

public interface IProjectRepository
{
    Task<Project> AddAsync(Project project, CancellationToken cancellationToken);
    Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Project>> GetForUserAsync(string userId, CancellationToken cancellationToken);
    Task AddMemberAsync(Guid projectId, string userId, CancellationToken cancellationToken);
}
