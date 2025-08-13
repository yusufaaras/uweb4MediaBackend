using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment;

public class CreateStripePaymentIntentCommandHandler : IRequestHandler<CreateStripePaymentIntentCommand, string>
{
    private readonly IPaymentRepository _repository;
    private readonly IStripePaymentService _stripeService;
    private readonly IAppUserRepository _userRepository;

    public CreateStripePaymentIntentCommandHandler(
        IPaymentRepository repository,
        IStripePaymentService stripeService,
        IAppUserRepository userRepository)
    {
        _repository = repository;
        _stripeService = stripeService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(CreateStripePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        if (!user.IsEmailVerified)
            throw new UnauthorizedAccessException("Mail doğrulanmadan ödeme yapılamaz!");

        var result = await _stripeService.CreatePaymentIntentAsync(
            request.Amount,
            request.OrderId,
            request.Email,
            request.Currency ?? "usd"
        );

        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Payment
        {
            OrderId = request.OrderId,
            StripePaymentIntentId = result.PaymentIntentId,
            Amount = request.Amount,
            Currency = request.Currency ?? "usd",
            Status = "pending",
            Provider = "stripe",
            Email = request.Email,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            IsToken = request.IsToken
        });
        if (request.IsToken)
        {
            user.PostToken += 5;
            await _userRepository.UpdateAsync(user);
        }

        return result.ClientSecret; 
    }
}