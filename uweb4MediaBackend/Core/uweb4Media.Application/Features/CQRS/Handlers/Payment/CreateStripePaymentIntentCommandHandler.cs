using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment;

public class CreateStripePaymentIntentCommandHandler : IRequestHandler<CreateStripePaymentIntentCommand, string>
{
    private readonly IPaymentRepository _repository;
    private readonly IStripePaymentService _stripeService;

    public CreateStripePaymentIntentCommandHandler(IPaymentRepository repository, IStripePaymentService stripeService)
    {
        _repository = repository;
        _stripeService = stripeService;
    }

    public async Task<string> Handle(CreateStripePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var result = await _stripeService.CreatePaymentIntentAsync(request.Amount, request.OrderId, request.Email);
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Payment
        {
            OrderId = request.OrderId,
            StripePaymentIntentId = result.PaymentIntentId,
            Amount = request.Amount,
            Currency = "TRY",
            Status = "pending",
            Provider = "stripe",
            Email = request.Email,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        });
        return result.ClientSecret;
    }
}