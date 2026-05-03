namespace TaskManagement.Domain.Domain;

public sealed class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<ProjectMember> Members { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new();
}
