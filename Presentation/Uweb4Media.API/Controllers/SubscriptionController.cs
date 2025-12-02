using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Subscription;
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Subscription;
using uweb4Media.Application.Features.CQRS.Queries.Subscription;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class SubscriptionController : ControllerBase
    {
        private readonly GetSubscriptionQueryHandler _getSubscriptionQueryHandler;
        private readonly GetSubscriptionByIdQueryHandler _getSubscriptionByIdQueryHandler;
        private readonly CreateSubscriptionCommandHandler _createSubscriptionCommandHandler;
        private readonly RemoveSubscriptionCommandHandler _removeSubscriptionCommandHandler;

        public SubscriptionController(GetSubscriptionQueryHandler getSubscriptionQueryHandler, GetSubscriptionByIdQueryHandler getSubscriptionByIdQueryHandler, CreateSubscriptionCommandHandler createSubscriptionCommandHandler, RemoveSubscriptionCommandHandler removeSubscriptionCommandHandler)
        {
            _getSubscriptionQueryHandler = getSubscriptionQueryHandler;
            _getSubscriptionByIdQueryHandler = getSubscriptionByIdQueryHandler;
            _createSubscriptionCommandHandler = createSubscriptionCommandHandler;
            _removeSubscriptionCommandHandler = removeSubscriptionCommandHandler;
        }
        [HttpGet]
        public async Task<IActionResult> SubscriptionList()
        {
            var values = await _getSubscriptionQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var value = await _getSubscriptionByIdQueryHandler.Handle(new GetSubscriptionByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionCommand command)
        {
            await _createSubscriptionCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSubscription(int id)
        {
            await _removeSubscriptionCommandHandler.Handle(new RemoveSubscriptionCommand(id));
            return Ok();
        }
    }
}