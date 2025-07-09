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
            var user = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            if (user == null)
                return BadRequest(new { message = "Email already exists." });

            return Ok(new {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                message = "Registered successfully."
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Email, request.Password);
            if (user == null) 
                return Unauthorized(new { message = "Invalid credentials." });

            return Ok(new { 
                id = user.Id, 
                username = user.Username, 
                email = user.Email,
                token = "dummy-token-if-you-have" // Eğer JWT eklediysen
            });
        }

        public class LoginRequest
        {
            public string Email { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        public class RegisterRequest
        {
            public string Username { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string Password { get; set; } = default!;
        }
    }
}
