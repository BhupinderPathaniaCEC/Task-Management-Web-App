using DomainTaskStatus = TaskManagement.Domain.Domain.TaskStatus;

namespace TaskManagement.Api.Contracts.Tasks;

public sealed record TaskResponse(Guid Id, Guid ProjectId, string Title, string? Description, DomainTaskStatus Status, DateOnly? DueDate, string? AssigneeUserId, DateTimeOffset CreatedAt);
