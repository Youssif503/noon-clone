using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using noon.Application.DTOs;
using noon.Application.Helpers;
using noon.Application.Service.Contract;
using noon.Domain.Models.Identity;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace noon.Application.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly Jwt _jwt;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IOptions<Jwt> jwt,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateAccessTokenAsync(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        var roleClaims = userRoles.Select(role => new Claim("role", role));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Name, $"{user.First_Name} {user.Last_Name}")
        }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey!));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(_jwt.DurationInHour ?? 8);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public async Task<AuthTokensDto> CreateAuthTokensAsync(User user)
    {
        var accessTokenExpiresAt = DateTime.UtcNow.AddHours(_jwt.DurationInHour ?? 8);
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDurationInDays ?? 7);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenHash = HashToken(refreshToken);

        await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshTokenExpiresAt
        });

        await _unitOfWork.SaveChangesAsync();

        return new AuthTokensDto
        {
            FirstName = user.First_Name,
            LastName = user.Last_Name,
            Email = user.Email!,
            AccessToken = await CreateAccessTokenAsync(user),
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    public async Task<AuthTokensDto?> RefreshTokensAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var tokenHash = HashToken(refreshToken);
        var storedToken = await _unitOfWork.RefreshTokens.GetActiveByTokenHashAsync(tokenHash);

        if (storedToken?.User is null || !storedToken.IsActive)
            return null;

        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenHash = HashToken(newRefreshToken);
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDurationInDays ?? 7);

        _unitOfWork.RefreshTokens.Revoke(storedToken, newRefreshTokenHash);

        await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = storedToken.UserId,
            TokenHash = newRefreshTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshTokenExpiresAt
        });

        await _unitOfWork.SaveChangesAsync();

        var user = storedToken.User;
        var accessTokenExpiresAt = DateTime.UtcNow.AddHours(_jwt.DurationInHour ?? 8);

        return new AuthTokensDto
        {
            FirstName = user.First_Name,
            LastName = user.Last_Name,
            Email = user.Email!,
            AccessToken = await CreateAccessTokenAsync(user),
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var tokenHash = HashToken(refreshToken);
        var storedToken = await _unitOfWork.RefreshTokens.GetActiveByTokenHashAsync(tokenHash);

        if (storedToken is null)
            return false;

        _unitOfWork.RefreshTokens.Revoke(storedToken);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeAllRefreshTokensAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        await _unitOfWork.RefreshTokens.RevokeAllForUserAsync(userId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
