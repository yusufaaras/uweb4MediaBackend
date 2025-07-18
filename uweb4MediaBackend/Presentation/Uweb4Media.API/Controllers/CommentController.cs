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
        private readonly GetCommentsByMediaContentIdQueryHandler _getCommentsByMediaContentIdQueryHandler;

        public CommentController(GetCommentQueryHandler getCommentQueryHandler, GetCommentByIdQueryHandler getCommentByIdQueryHandler, CreateCommentCommandHandler createCommentCommandHandler, RemoveCommentCommandHandler removeCommentCommandHandler, GetCommentsByMediaContentIdQueryHandler getCommentsByMediaContentIdQueryHandler)
        {
            _getCommentQueryHandler = getCommentQueryHandler;
            _getCommentByIdQueryHandler = getCommentByIdQueryHandler;
            _createCommentCommandHandler = createCommentCommandHandler;
            _removeCommentCommandHandler = removeCommentCommandHandler;
            _getCommentsByMediaContentIdQueryHandler = getCommentsByMediaContentIdQueryHandler;
        } 
        [HttpGet]
        public async Task<IActionResult> CommentList([FromQuery] int? mediaContentId) // Query parametresi ekleyin
        {
            if (mediaContentId.HasValue)
            {
                var comments = await _getCommentsByMediaContentIdQueryHandler.Handle(new GetCommentsByMediaContentIdQuery(mediaContentId.Value));
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

