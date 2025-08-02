using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Comments;
using uweb4Media.Application.Features.CQRS.Handlers.Comments;
using uweb4Media.Application.Features.CQRS.Queries.Comments;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class CommentController : ControllerBase
    {
        private readonly GetCommentQueryHandler _getCommentQueryHandler;
        private readonly GetCommentByIdQueryHandler _getCommentByIdQueryHandler;
        private readonly CreateCommentCommandHandler _createCommentCommandHandler;
        private readonly RemoveCommentCommandHandler _removeCommentCommandHandler;
        private readonly GetCommentsByVideoIdQueryHandler _getCommentsByVideoIdQueryHandler;

        public CommentController(GetCommentQueryHandler getCommentQueryHandler, GetCommentByIdQueryHandler getCommentByIdQueryHandler, CreateCommentCommandHandler createCommentCommandHandler, RemoveCommentCommandHandler removeCommentCommandHandler, GetCommentsByVideoIdQueryHandler getCommentsByVideoIdQueryHandler)
        {
            _getCommentQueryHandler = getCommentQueryHandler;
            _getCommentByIdQueryHandler = getCommentByIdQueryHandler;
            _createCommentCommandHandler = createCommentCommandHandler;
            _removeCommentCommandHandler = removeCommentCommandHandler;
            _getCommentsByVideoIdQueryHandler = getCommentsByVideoIdQueryHandler;
        } 
        [HttpGet]
        public async Task<IActionResult> CommentList([FromQuery] int? VideoId) // Query parametresi ekleyin
        {
            if (VideoId.HasValue)
            {
                var comments = await _getCommentsByVideoIdQueryHandler.Handle(new GetCommentsByVideoIdQuery(VideoId.Value));
                return Ok(comments);
            }
            else
            {
                var values = await _getCommentQueryHandler.Handle();
                return Ok(values);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var value = await _getCommentByIdQueryHandler.Handle(new GetCommentByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentCommand command)
        {
            await _createCommentCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveComment(int id)
        {
            await _removeCommentCommandHandler.Handle(new RemoveCommentCommand(id));
            return Ok();
        }
    }
}

