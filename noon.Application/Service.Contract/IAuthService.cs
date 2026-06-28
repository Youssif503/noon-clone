using noon.Application.DTOs;
using noon.Domain.Models.Identity;

namespace noon.Application.Service.Contract;

public interface IAuthService
{
    Task<string> CreateAccessTokenAsync(User user);
    Task<AuthTokensDto> CreateAuthTokensAsync(User user);
    Task<AuthTokensDto?> RefreshTokensAsync(string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    Task<bool> RevokeAllRefreshTokensAsync(string userId);
}
