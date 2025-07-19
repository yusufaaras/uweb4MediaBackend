using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Features.CQRS.Handlers.Subscription;
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Features.CQRS.Queries.User;

namespace Uweb4Media.API.Controllers;

// GET
[Route("api/[controller]")]
[ApiController] 
public class UserSubscribeStatusController : ControllerBase
{
    private readonly GetSubscribeUserByIdQueryHandler _getSubscriptionQueryHandler;
        private readonly UpdateSubscribeUserCommandHandler _updateSubscribeUserCommandHandler;


        public UserSubscribeStatusController(GetSubscribeUserByIdQueryHandler getSubscriptionQueryHandler, UpdateSubscribeUserCommandHandler updateSubscribeUserCommandHandler)
        {
            _getSubscriptionQueryHandler = getSubscriptionQueryHandler;
            _updateSubscribeUserCommandHandler = updateSubscribeUserCommandHandler;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var value = await _getSubscriptionQueryHandler.Handle(new GetSubscribeUserByIdQuery(id));
            return Ok(value);
        } 
        [HttpPut]
        public async Task<IActionResult> UpdateUser(SubscribeUserCommand command)
        {
            await _updateSubscribeUserCommandHandler.Handle(command);
            return Ok("Kullanıcı Başarıyla Güncellendi");
        }
}