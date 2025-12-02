using MediatR;

namespace uweb4Media.Application.Features.CQRS.Commands.Payment;

public class CreateStripeIntentWithCodeCommand : IRequest<string>
{
    public int PaymentId { get; set; }
    public string Code { get; set; }
}