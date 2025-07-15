using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Media;
using uweb4Media.Application.Features.CQRS.Handlers.Media;
using uweb4Media.Application.Features.CQRS.Queries.Media;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MediaController : ControllerBase
    {
        
            private readonly GetMediaContentQueryHandler _getMediaContentQueryHandler;
            private readonly GetMediaContentByIdQueryHandler _getMediaContentByIdQueryHandler;
            private readonly CreateMediaContentCommandHandler _createMediaContentCommandHandler;
            private readonly UpdateMediaContentCommandHandler _updateMediaContentCommandHandler;
            private readonly RemoveMediaContentCommandHandler _removeMediaContentCommandHandler;
    
            public MediaController(GetMediaContentQueryHandler getMediaContentQueryHandler, GetMediaContentByIdQueryHandler getMediaContentByIdQueryHandler, CreateMediaContentCommandHandler createMediaContentCommandHandler, UpdateMediaContentCommandHandler updateMediaContentCommandHandler, RemoveMediaContentCommandHandler removeMediaContentCommandHandler)
            {
                _getMediaContentQueryHandler = getMediaContentQueryHandler;
                _getMediaContentByIdQueryHandler = getMediaContentByIdQueryHandler;
                _createMediaContentCommandHandler = createMediaContentCommandHandler;
                _updateMediaContentCommandHandler = updateMediaContentCommandHandler;
                _removeMediaContentCommandHandler = removeMediaContentCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> MediaContentList()
            {
                var values = await _getMediaContentQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetMediaContent(int id)
            {
                var value = await _getMediaContentByIdQueryHandler.Handle(new GetMediaContentByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreateMediaContent(CreateMediaContentCommand command)
            {
                await _createMediaContentCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemoveMediaContent(int id)
            {
                await _removeMediaContentCommandHandler.Handle(new RemoveMediaContentCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdateMediaContent(UpdateMediaContentCommand command)
            {
                await _updateMediaContentCommandHandler.Handle(command);
                return Ok();
            }
    }
}

