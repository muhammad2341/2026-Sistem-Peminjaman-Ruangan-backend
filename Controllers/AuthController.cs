using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.DTOs;
using RoomBooking.Api.Models;
using RoomBooking.Api.Services;

namespace RoomBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _configuration.GetSection("Auth:Users").Get<List<AuthUser>>() ?? new List<AuthUser>();

            var user = users.FirstOrDefault(u =>
                string.Equals(u.Username, request.Username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == request.Password
            );

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = _tokenService.GenerateToken(user);
            var expiryMinutes = _tokenService.GetTokenExpiryMinutes();

            var response = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.Username : user.DisplayName,
                Role = user.Role,
                ExpiresInSeconds = expiryMinutes * 60
            };

            return Ok(response);
        }
    }
}
