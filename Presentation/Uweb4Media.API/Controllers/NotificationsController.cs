using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Notification;
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Features.CQRS.Queries.Notification;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class NotificationsController : ControllerBase
    {
        private readonly GetNotificationQueryHandler _getNotificationQueryHandler;
        private readonly GetNotificationByIdQueryHandler _getNotificationByIdQueryHandler;
        private readonly CreateNotificationCommandHandler _createNotificationCommandHandler;
        private readonly RemoveNotificationCommandHandler _removeNotificationCommandHandler;

        public NotificationsController(GetNotificationQueryHandler getNotificationQueryHandler, GetNotificationByIdQueryHandler getNotificationByIdQueryHandler, CreateNotificationCommandHandler createNotificationCommandHandler, RemoveNotificationCommandHandler removeNotificationCommandHandler)
        {
            _getNotificationQueryHandler = getNotificationQueryHandler;
            _getNotificationByIdQueryHandler = getNotificationByIdQueryHandler;
            _createNotificationCommandHandler = createNotificationCommandHandler;
            _removeNotificationCommandHandler = removeNotificationCommandHandler;
        }
        [HttpGet]
        public async Task<IActionResult> NotificationList(CancellationToken cancellationToken)
        { 
            var values = await _getNotificationQueryHandler.Handle(new GetNotificationQuery(), cancellationToken);
            return Ok(values);
        }
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetNotification(int id, CancellationToken cancellationToken)
        { 
            var value = await _getNotificationByIdQueryHandler.Handle(new GetNotificationByIdQuery(id), cancellationToken);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(CreateNotificationCommand command)
        {
            await _createNotificationCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveNotification(int id)
        {
            await _removeNotificationCommandHandler.Handle(new RemoveNotificationCommand(id));
            return Ok();
        }
    }
}