using MediatR;
using Microsoft.Extensions.Caching.Memory;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Interfaces.Email;
using Uweb4Media.Domain.Entities;
using uweb4Media.Application.Model;

namespace uweb4Media.Application.Features.Mediator.Handlers;

public class RegisterStartCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;
    private readonly IRepository<AppUser> _repository;

    public RegisterStartCommandHandler(IMemoryCache cache, IEmailService emailService, IRepository<AppUser> repository)
    {
        _cache = cache;
        _emailService = emailService;
        _repository = repository;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByFilterAsync(
            x => x.Username == request.Username || x.Email == request.Email);

        if (existingUser != null)
            throw new Exception("Aynı kullanıcı adı veya e-posta ile kayıt yapılamaz.");

        if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
            throw new Exception("Geçerli bir e-posta adresi giriniz.");

        var code = new Random().Next(100000, 999999).ToString();
        var temp = new TempRegisterModel
        {
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            VerificationCode = code
        };
        _cache.Set(request.Email, temp, TimeSpan.FromMinutes(10));

        await _emailService.SendEmailAsync(request.Email, "Doğrulama Kodunuz", $"Kodunuz: {code}");
        return true;
    }
}