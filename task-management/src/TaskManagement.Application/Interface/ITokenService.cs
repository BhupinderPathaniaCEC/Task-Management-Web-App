using TaskManagement.Domain.Domain;

namespace TaskManagement.Application.Interface;

public interface ITokenService
{
    Task<string> CreateJwtAsync(ApplicationUser user, CancellationToken cancellationToken);
}
