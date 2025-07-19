using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Features.CQRS.Queries.User;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class UserController : Controller
    {
        private readonly GetUserQueryHandler _getUserQueryHandler;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly RemoveUserCommandHandler _removeUserCommandHandler;
        private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;

        public UserController(GetUserQueryHandler getUserQueryHandler, UpdateUserCommandHandler updateUserCommandHandler, RemoveUserCommandHandler removeUserCommandHandler, GetUserByIdQueryHandler getUserByIdQueryHandler)
        {
            _getUserQueryHandler = getUserQueryHandler;
            _updateUserCommandHandler = updateUserCommandHandler;
            _removeUserCommandHandler = removeUserCommandHandler;
            _getUserByIdQueryHandler = getUserByIdQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var values = await _getUserQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var value = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery(id));
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            await _removeUserCommandHandler.Handle(new RemoveUserCommand(id));
            return Ok("Kullanıcı Başarıyla Silindi");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            await _updateUserCommandHandler.Handle(command);
            return Ok("Kullanıcı Başarıyla Güncellendi");
        }
    }
}
