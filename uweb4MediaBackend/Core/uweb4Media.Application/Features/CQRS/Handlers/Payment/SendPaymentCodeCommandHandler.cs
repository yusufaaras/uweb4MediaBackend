using MediatR;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Interfaces.Email;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment;

public class SendPaymentCodeCommandHandler : IRequestHandler<SendPaymentCodeCommand, int>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAppUserRepository _userRepository;
    private readonly IEmailService _mailService; //  

    public SendPaymentCodeCommandHandler(
        IPaymentRepository paymentRepository,
        IAppUserRepository userRepository,
        IEmailService mailService)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _mailService = mailService;
    }

    public async Task<int> Handle(SendPaymentCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null || !user.IsEmailVerified)
            throw new UnauthorizedAccessException("Mail doğrulanmadan ödeme kodu alınamaz!");

        var code = new Random().Next(100000, 999999).ToString();

        // Mail gönder
        await _mailService.SendEmailAsync(request.Email, "Ödeme Doğrulama Kodu", $"Kodunuz: {code}");

        // Payment kaydı (intent yok, sadece code ve status)
        var payment = new Uweb4Media.Domain.Entities.Payment
        {
            OrderId = request.OrderId,
            Amount = request.Amount,
            Currency = request.Currency ?? "usd",
            Status = "awaiting_code",
            Provider = "stripe",
            Email = request.Email,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            PaymentCode = code,
            PaymentCodeGeneratedAt = DateTime.UtcNow,
        };
        await _paymentRepository.CreateAsync(payment);

        return payment.Id;
    }
}