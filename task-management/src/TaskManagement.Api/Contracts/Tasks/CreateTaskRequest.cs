using TaskManagement.Domain.Domain;

namespace TaskManagement.Api.Contracts.Tasks;

public sealed record CreateTaskRequest(Guid ProjectId, string Title, string? Description, string? AssigneeUserId, DateOnly? DueDate);
