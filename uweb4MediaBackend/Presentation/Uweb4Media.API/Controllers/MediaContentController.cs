using Microsoft.AspNetCore.Mvc;
using MediatR;
using Uweb4Media.Application.Features.MediaContents.Commands;
using Uweb4Media.Application.Features.MediaContents.Queries;

namespace Uweb4Media.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaContentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromBody] CreateMediaContentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMediaContentsQuery());
            return Ok(result);
        }
    }
}