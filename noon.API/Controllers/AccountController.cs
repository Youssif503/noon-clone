using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using noon.Application.DTOs;
using noon.Application.Service.Contract;
using noon.Domain.Models.Identity;
using System.Security.Claims;

namespace noon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            IAuthService authService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            var existUser = await _userManager.FindByEmailAsync(user.Email);

            if (existUser is not null)
                return Conflict("Email Alredy Exist");

            var newUser = new User
            {
                Email = user.Email,
                First_Name = user.FirstName,
                Last_Name = user.LastName,
                UserName = user.Email,
            };

            var createResult = await _userManager.CreateAsync(newUser, user.Password);

            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            var tokens = await _authService.CreateAuthTokensAsync(newUser);
            return StatusCode(StatusCodes.Status201Created, MapToUserDto(tokens));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var existUser = await _userManager.FindByEmailAsync(user.Email);

            if (existUser is null)
                return Unauthorized("Invalid Email Or Password");

            var checkPasswordResult =
                await _signInManager.CheckPasswordSignInAsync(existUser, user.Password, false);

            if (!checkPasswordResult.Succeeded)
                return Unauthorized("Invalid Email Or Password");

            var tokens = await _authService.CreateAuthTokensAsync(existUser);
            return Ok(MapToUserDto(tokens));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("Refresh token is required");

            var tokens = await _authService.RefreshTokensAsync(request.RefreshToken);

            if (tokens is null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(MapToUserDto(tokens));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("Refresh token is required");

            var revoked = await _authService.RevokeRefreshTokenAsync(request.RefreshToken);

            if (!revoked)
                return NotFound("Refresh token not found or already revoked");

            return NoContent();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAllDevices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _authService.RevokeAllRefreshTokensAsync(userId);
            return NoContent();
        }

        private static UserDto MapToUserDto(AuthTokensDto tokens) =>
            new()
            {
                Email = tokens.Email,
                FirstName = tokens.FirstName,
                LastName = tokens.LastName,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                AccessTokenExpiresAt = tokens.AccessTokenExpiresAt,
                RefreshTokenExpiresAt = tokens.RefreshTokenExpiresAt
            };
    }
}
