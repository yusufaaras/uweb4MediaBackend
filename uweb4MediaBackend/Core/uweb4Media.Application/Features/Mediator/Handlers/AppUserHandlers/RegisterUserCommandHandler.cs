using MediatR;
using Microsoft.Extensions.Caching.Memory;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces.Email;
using uweb4Media.Application.Model;

namespace uweb4Media.Application.Features.Mediator.Handlers.AppUserHandlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(IMemoryCache cache, IEmailService emailService)
    {
        _cache = cache;
        _emailService = emailService;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Benzersiz kontrolü ve validasyon burada yapılmalı (kendi repository'de kontrol edebilirsin)
        if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
            throw new Exception("Geçerli bir e-posta adresi giriniz.");

        var verificationCode = new Random().Next(100000, 999999).ToString();
        var cacheModel = new TempRegisterModel
        {
            Username = request.Username,
            Password = request.Password, // hashlemeden, çünkü kayıt tamamlandığında hashleyeceğiz!
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            VerificationCode = verificationCode
        };

        _cache.Set(request.Email, cacheModel, TimeSpan.FromMinutes(10));
        await _emailService.SendEmailAsync(request.Email, "Doğrulama Kodunuz", $"Kodunuz: {verificationCode}");
        return true;
    }
}