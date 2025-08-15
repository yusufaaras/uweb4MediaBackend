using MediatR;
using Microsoft.Extensions.Caching.Memory;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Helper;
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
        
        if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
            throw new Exception("Geçerli bir e-posta adresi giriniz.");
        
        if (!EmailDomainHelper.IsAllowedDomain(request.Email))
            throw new Exception("Bu e-posta sağlayıcısına izin verilmiyor.");
        
        var verificationCode = new Random().Next(100000, 999999).ToString();
        var cacheModel = new TempRegisterModel
        {
            Username = request.Username,
            Password = request.Password, 
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