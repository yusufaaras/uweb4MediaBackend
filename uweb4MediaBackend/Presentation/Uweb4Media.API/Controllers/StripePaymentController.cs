using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe; 
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.Payment;

namespace Uweb4Media.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripePaymentController : Controller
{
    private readonly IMediator _mediator;
    private readonly IPaymentRepository _paymentRepo;
    private readonly IConfiguration _config;

    public StripePaymentController(IMediator mediator, IPaymentRepository paymentRepo, IConfiguration config)
    {
        _mediator = mediator;
        _paymentRepo = paymentRepo;
        _config = config;
    }

    [HttpPost("create-stripe-intent")]
    public async Task<IActionResult> CreateStripeIntent([FromBody] CreateStripePaymentIntentCommand command)
    {
        var clientSecret = await _mediator.Send(command);
        return Ok(new { clientSecret });
    }

    [HttpPost("stripe-webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            _config["Stripe:WebhookSecret"]
        );

        if (stripeEvent.Type == "payment_intent.succeeded")

    {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            var payment = await _paymentRepo.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
            if (payment != null)
            {
                payment.Status = "success";
                await _paymentRepo.UpdateAsync(payment);
            }
        }

        return Ok();
    }
}