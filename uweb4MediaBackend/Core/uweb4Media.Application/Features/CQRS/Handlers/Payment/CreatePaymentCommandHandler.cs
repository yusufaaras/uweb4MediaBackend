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

        public CreatePaymentCommandHandler(IPaymentService paymentService, IRepository<Uweb4Media.Domain.Entities.Payment> repository)
        {
            _paymentService = paymentService;
            _repository = repository;
        }

        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentId = await _paymentService.CreatePaymentAsync(
                request.Amount,
                request.OrderId,
                request.Email,
                request.CardHolderName,
                request.CardNumber,
                request.ExpireMonth,
                request.ExpireYear,
                request.Cvc
            );

            // Sadece başarılı ödeme olursa kaydet
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
                    CreatedAt = DateTime.UtcNow
                });
            };

            return paymentId;
            
        }
    }
}