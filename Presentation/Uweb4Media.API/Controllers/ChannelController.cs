using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Channel;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly GetChannelQueryHandler _getChannelQueryHandler;
        private readonly GetChannelByIdQueryHandler _getChannelByIdQueryHandler;
        private readonly CreateChannelCommandHandler _createChannelCommandHandler;
        private readonly UpdateChannelCommandHandler _updateChannelCommandHandler;
        private readonly RemoveChannelCommandHandler _removeChannelCommandHandler;

        public ChannelController(
            GetChannelQueryHandler getChannelQueryHandler,
            GetChannelByIdQueryHandler getChannelByIdQueryHandler,
            CreateChannelCommandHandler createChannelCommandHandler,
            UpdateChannelCommandHandler updateChannelCommandHandler,
            RemoveChannelCommandHandler removeChannelCommandHandler)
        {
            _getChannelQueryHandler = getChannelQueryHandler;
            _getChannelByIdQueryHandler = getChannelByIdQueryHandler;
            _createChannelCommandHandler = createChannelCommandHandler;
            _updateChannelCommandHandler = updateChannelCommandHandler;
            _removeChannelCommandHandler = removeChannelCommandHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChannelList()
        {
            var values = await _getChannelQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChannel(int id)
        {
            var value = await _getChannelByIdQueryHandler.Handle(new GetChannelByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateChannel(CreateChannelCommand command)
        {
            await _createChannelCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveChannel(int id)
        {
            await _removeChannelCommandHandler.Handle(new RemoveChannelCommand(id));
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateChannel(UpdateChannelCommand command)
        {
            await _updateChannelCommandHandler.Handle(command);
            return Ok();
        }
    }
}