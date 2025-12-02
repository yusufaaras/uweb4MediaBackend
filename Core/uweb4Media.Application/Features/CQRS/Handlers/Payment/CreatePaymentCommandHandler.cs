using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepository<Uweb4Media.Domain.Entities.Payment> _repository;
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<Uweb4Media.Domain.Entities.Plans> _plansRepository; // <-- EKLENDİ

        public CreatePaymentCommandHandler(IPaymentService paymentService,
            IRepository<Uweb4Media.Domain.Entities.Payment> repository,
            IRepository<AppUser> userRepository,
            IRepository<Uweb4Media.Domain.Entities.Plans> plansRepository) // <-- EKLENDİ
        {
            _paymentService = paymentService;
            _repository = repository;
            _userRepository = userRepository;
            _plansRepository = plansRepository; // <-- EKLENDİ
        }

        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. iyzico ile ödeme isteği
            var paymentId = await _paymentService.CreatePaymentAsync(
                request.Amount, request.OrderId, request.Email,
                request.CardHolderName, request.CardNumber, request.ExpireMonth, request.ExpireYear, request.Cvc
            );

            // 2. Başarılı ise db'ye kaydet
            if (!string.IsNullOrEmpty(paymentId))
            {
                await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Payment
                {
                    OrderId = request.OrderId,
                    IyzicoPaymentId = paymentId,
                    Amount = request.Amount,
                    Currency = "TRY",
                    Status = "success",
                    Email = request.Email,
                    UserId = request.UserId,
                    IsToken = request.IsToken,
                    CreatedAt = DateTime.UtcNow
                });

                if (request.IsToken)
                {
                    // PlanId bilgisini de CreatePaymentCommand'a eklemiş olmalısın!
                    var plan = await _plansRepository.GetByIdAsync(request.PlanId);
                    int tokenCount = 1;
                    if (plan != null && plan.IsToken && plan.TokenCount.HasValue)
                    {
                        tokenCount = plan.TokenCount.Value;
                    }

                    var user = await _userRepository.GetByIdAsync(request.UserId);
                    if (user != null)
                    {
                        user.PostToken += tokenCount;
                        await _userRepository.UpdateAsync(user);
                    }
                }
            }

            return paymentId;
        }
    }
}