using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment;

public class CreateStripeIntentWithCodeCommandHandler : IRequestHandler<CreateStripeIntentWithCodeCommand, string>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStripePaymentService _stripeService;

    public CreateStripeIntentWithCodeCommandHandler(
        IPaymentRepository paymentRepository,
        IStripePaymentService stripeService)
    {
        _paymentRepository = paymentRepository;
        _stripeService = stripeService;
    }

    public async Task<string> Handle(CreateStripeIntentWithCodeCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
        if (payment == null)
            throw new Exception("Ödeme kaydı bulunamadı.");

        if (payment.PaymentCode != request.Code)
            throw new Exception("Kod hatalı.");

        var result = await _stripeService.CreatePaymentIntentAsync(payment.Amount, payment.OrderId, payment.Email, payment.Currency ?? "usd");
        payment.StripePaymentIntentId = result.PaymentIntentId;
        payment.Status = "pending";
        await _paymentRepository.UpdateAsync(payment);

        return result.ClientSecret;
    }
}