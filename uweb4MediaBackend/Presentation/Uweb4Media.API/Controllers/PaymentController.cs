using MediatR;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Payment;

namespace Uweb4Media.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        try
        {
            var paymentId = await _mediator.Send(command);
            return Ok(new { paymentId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}