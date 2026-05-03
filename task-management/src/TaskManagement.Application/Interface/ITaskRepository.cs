using TaskManagement.Domain.Domain;

namespace TaskManagement.Application.Interface;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken);
    Task<TaskItem?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskItem>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task UpdateAsync(TaskItem task, CancellationToken cancellationToken);
}
