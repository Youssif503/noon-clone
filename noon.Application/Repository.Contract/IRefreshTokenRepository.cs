using noon.Domain.Models.Identity;

namespace noon.Application.Repository.Contract;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetActiveByTokenHashAsync(string tokenHash);
    void Revoke(RefreshToken refreshToken, string? replacedByTokenHash = null);
    Task RevokeAllForUserAsync(string userId);
}
