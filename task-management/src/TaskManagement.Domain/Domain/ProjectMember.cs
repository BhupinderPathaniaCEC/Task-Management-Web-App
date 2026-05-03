namespace TaskManagement.Domain.Domain;

public sealed class ProjectMember
{
    public Guid ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public Project? Project { get; set; }
    public ApplicationUser? User { get; set; }
}
