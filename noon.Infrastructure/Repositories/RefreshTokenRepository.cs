using Microsoft.EntityFrameworkCore;
using noon.Application.Repository.Contract;
using noon.Domain.Models.Identity;
using noon.Infrastructure.Data;

namespace noon.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetActiveByTokenHashAsync(string tokenHash)
    {
        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash &&
                rt.RevokedAt == null &&
                rt.ExpiresAt > DateTime.UtcNow);
    }

    public void Revoke(RefreshToken refreshToken, string? replacedByTokenHash = null)
    {
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.ReplacedByTokenHash = replacedByTokenHash;
        _dbContext.RefreshTokens.Update(refreshToken);
    }

    public async Task RevokeAllForUserAsync(string userId)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }
    }
}
