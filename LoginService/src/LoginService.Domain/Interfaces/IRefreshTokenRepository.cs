using LoginService.Domain.Entities;

namespace LoginService.Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
    Task RevokeAllUserTokensAsync(int userId, string revokedByIp);
}
