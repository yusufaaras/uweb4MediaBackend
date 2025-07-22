using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Video;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        
            private readonly GetVideoQueryHandler _getVideoQueryHandler;
            private readonly GetVideoByIdQueryHandler _getVideoByIdQueryHandler;
            private readonly CreateVideoCommandHandler _createVideoCommandHandler;
            private readonly UpdateVideoCommandHandler _updateVideoCommandHandler;
            private readonly RemoveVideoCommandHandler _removeVideoCommandHandler;
    
            public VideoController(GetVideoQueryHandler getVideoQueryHandler, GetVideoByIdQueryHandler getVideoByIdQueryHandler, CreateVideoCommandHandler createVideoCommandHandler, UpdateVideoCommandHandler updateVideoCommandHandler, RemoveVideoCommandHandler removeVideoCommandHandler)
            {
                _getVideoQueryHandler = getVideoQueryHandler;
                _getVideoByIdQueryHandler = getVideoByIdQueryHandler;
                _createVideoCommandHandler = createVideoCommandHandler;
                _updateVideoCommandHandler = updateVideoCommandHandler;
                _removeVideoCommandHandler = removeVideoCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> VideoList()
            {
                var values = await _getVideoQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetVideo(int id)
            {
                var value = await _getVideoByIdQueryHandler.Handle(new GetVideoByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreateVideo(CreateVideoCommand command)
            {
                await _createVideoCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemoveVideo(int id)
            {
                await _removeVideoCommandHandler.Handle(new RemoveVideoCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdateVideo(UpdateVideoCommand command)
            {
                await _updateVideoCommandHandler.Handle(command);
                return Ok();
            }
    }
}