using MediatR;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RegistersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateAppUserCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("The User Was Successfully Added. Please check your email for the verification code.");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Bir hata oluştu: {errorMessage}");
            }
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok("Email verified successfully.");
            else
                return BadRequest("Invalid verification code or email.");
        }
    }
}
