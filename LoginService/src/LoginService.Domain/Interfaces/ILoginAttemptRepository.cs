using LoginService.Domain.Entities;

namespace LoginService.Domain.Interfaces;

public interface ILoginAttemptRepository : IRepository<LoginAttempt>
{
    Task<IEnumerable<LoginAttempt>> GetByUserIdAsync(int userId);
    Task<int> GetFailedAttemptsCountAsync(int userId, TimeSpan timeWindow);
}
