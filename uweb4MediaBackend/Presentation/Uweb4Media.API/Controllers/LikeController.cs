using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Queries.Like;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly GetLikeQueryHandler _getLikeQueryHandler;
        private readonly GetLikeByIdQueryHandler _getLikeByIdQueryHandler;
        private readonly CreateLikeCommandHandler _createLikeCommandHandler;
        private readonly RemoveLikeCommandHandler _removeLikeCommandHandler;

        public LikeController(GetLikeQueryHandler getLikeQueryHandler, GetLikeByIdQueryHandler getLikeByIdQueryHandler, CreateLikeCommandHandler createLikeCommandHandler, RemoveLikeCommandHandler removeLikeCommandHandler)
        {
            _getLikeQueryHandler = getLikeQueryHandler;
            _getLikeByIdQueryHandler = getLikeByIdQueryHandler;
            _createLikeCommandHandler = createLikeCommandHandler;
            _removeLikeCommandHandler = removeLikeCommandHandler;
        }
        [HttpGet]
        public async Task<IActionResult> LikeList()
        {
            var values = await _getLikeQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLike(int id)
        {
            var value = await _getLikeByIdQueryHandler.Handle(new GetLikeByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike(CreateLikeCommand command)
        {
            await _createLikeCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLike(int id)
        {
            await _removeLikeCommandHandler.Handle(new RemoveLikeCommand(id));
            return Ok();
        }
    }
}