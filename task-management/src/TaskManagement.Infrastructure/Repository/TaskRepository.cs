using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interface;
using TaskManagement.Domain.Domain;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repository;

public sealed class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync(cancellationToken);
        return task;
    }

    public Task<TaskItem?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        return _db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var tasks = await _db.Tasks
            .Where(x => x.ProjectId == projectId)
            .ToListAsync(cancellationToken);
        
        return tasks.OrderByDescending(x => x.CreatedAt).ToList();
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
