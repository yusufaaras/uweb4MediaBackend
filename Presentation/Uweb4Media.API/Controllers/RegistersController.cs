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

        [HttpPost("StartRegistration")]
        public async Task<IActionResult> StartRegistration(RegisterUserCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("Doğrulama kodu mailinize gönderildi.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration(VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok("Kayıt başarılı.");
            else
                return BadRequest("Kod yanlış veya süresi geçti.");
        }
    }
}
