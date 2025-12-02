using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment;

public class CreateStripePaymentIntentCommandHandler : IRequestHandler<CreateStripePaymentIntentCommand, string>
{
    private readonly IPaymentRepository _repository;
    private readonly IStripePaymentService _stripeService;
    private readonly IAppUserRepository _userRepository;
    private readonly IRepository<Uweb4Media.Domain.Entities.Plans> _plansRepository;

    public CreateStripePaymentIntentCommandHandler(IPaymentRepository repository, IStripePaymentService stripeService, IAppUserRepository userRepository, IRepository<Uweb4Media.Domain.Entities.Plans> plansRepository)
    {
        _repository = repository;
        _stripeService = stripeService;
        _userRepository = userRepository;
        _plansRepository = plansRepository;
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
            IsToken = request.IsToken,
            PlanId = request.PlanId  
        });

        if (request.IsToken)
        {
            if (request.PlanId == null)
                throw new ArgumentException("Token ödemesi için PlanId zorunludur!");

            var plan = await _plansRepository.GetByIdAsync(request.PlanId.Value);
            int tokenCount = 1;
            if (plan != null && plan.IsToken && plan.TokenCount.HasValue)
                tokenCount = plan.TokenCount.Value;

            user.PostToken += tokenCount;
            await _userRepository.UpdateAsync(user);
        }
        else
        { 
            user.SubscriptionStatus = "premium";
            user.SubscriptionStartDate = DateTime.UtcNow;
            user.SubscriptionEndDate = DateTime.UtcNow.AddYears(1);
            await _userRepository.UpdateAsync(user);
        }

        return result.ClientSecret; 
    }
}