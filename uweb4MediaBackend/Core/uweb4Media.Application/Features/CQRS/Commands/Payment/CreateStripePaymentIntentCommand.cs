using MediatR;

namespace uweb4Media.Application.Features.CQRS.Commands.Payment;

public class CreateStripePaymentIntentCommand : IRequest<string>
{
    public decimal Amount { get; set; }
    public string OrderId { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    public string Currency { get; set; }
}