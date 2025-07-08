using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            bool success = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            if (!success) return BadRequest("Username already exists.");
            return Ok("Registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Username, request.Password);
            if (user == null) return Unauthorized("Invalid credentials.");
            return Ok(new { user.Id, user.Username, user.Email });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}