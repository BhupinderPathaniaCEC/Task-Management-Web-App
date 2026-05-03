namespace TaskManagement.Api.Contracts.Projects;

public sealed record ProjectResponse(Guid Id, string Name, string? Description, DateTimeOffset CreatedAt);
