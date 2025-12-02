using MediatR;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Queries;
using uweb4Media.Application.Tools;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CheckAppUserQuery(GetCheckAppUserQuery query)
        {
            var values = await _mediator.Send(query);
            if (values.IsExits)
            {
                return Created("", JwtTokenGenerator.GenerateToken(values));
            }
            else
            {
                return BadRequest("The username or password for uweb4 is incorrect");
            }
        }
    }
}
