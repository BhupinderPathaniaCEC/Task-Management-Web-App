using DomainTaskStatus = TaskManagement.Domain.Domain.TaskStatus;

namespace TaskManagement.Api.Contracts.Tasks;

public sealed record UpdateTaskStatusRequest(DomainTaskStatus Status);
