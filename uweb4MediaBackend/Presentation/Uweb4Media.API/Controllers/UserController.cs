using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Features.CQRS.Queries.User;
using Uweb4Media.Persistence.Context;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly GetUserQueryHandler _getUserQueryHandler;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly RemoveUserCommandHandler _removeUserCommandHandler;
        private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;
        private readonly Uweb4MediaContext _db;

        public UserController(GetUserQueryHandler getUserQueryHandler, UpdateUserCommandHandler updateUserCommandHandler, RemoveUserCommandHandler removeUserCommandHandler, GetUserByIdQueryHandler getUserByIdQueryHandler, Uweb4MediaContext db)
        {
            _getUserQueryHandler = getUserQueryHandler;
            _updateUserCommandHandler = updateUserCommandHandler;
            _removeUserCommandHandler = removeUserCommandHandler;
            _getUserByIdQueryHandler = getUserByIdQueryHandler;
            _db = db;
        }

        // Sadece admin görebilir
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var values = await _getUserQueryHandler.Handle();
            return Ok(values);
        }

        // Sadece admin görebilir
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            var value = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery(id));
            return Ok(value);
        }

        // Sadece admin silebilir
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            await _removeUserCommandHandler.Handle(new RemoveUserCommand(id));
            return Ok("Kullanıcı Başarıyla Silindi");
        }

        // Admin veya kullanıcı kendi hesabını güncelleyebilir
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value 
                           ?? User.FindFirst("role")?.Value 
                           ?? User.FindFirst("Role")?.Value;

            // LOG EKLE!
            Console.WriteLine($"userIdStr: {userIdStr}, userRole: {userRole}, command.AppUserID: {command.AppUserID}");

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            if (userRole == "Admin")
            {
                await _updateUserCommandHandler.Handle(command);
                return Ok("Kullanıcı Başarıyla Güncellendi");
            }
            if (command.AppUserID != userId)
                return Forbid();

            await _updateUserCommandHandler.Handle(command);
            return Ok("Profiliniz başarıyla güncellendi.");
        }
        
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var user = await _db.AppUsers.FindAsync(userId);
            if (user == null)
                return NotFound(); 
            if (user.SubscriptionStatus == "premium" && user.SubscriptionEndDate != null && user.SubscriptionEndDate < DateTime.UtcNow)
            {
                user.SubscriptionStatus = "free";
                user.SubscriptionStartDate = null;
                user.SubscriptionEndDate = null;
                await _db.SaveChangesAsync();
            }

            return Ok(user);  
        }
        
    }
}