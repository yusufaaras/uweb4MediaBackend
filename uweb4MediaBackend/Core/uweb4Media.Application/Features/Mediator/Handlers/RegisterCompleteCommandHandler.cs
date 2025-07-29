using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Model;using MediatR;
using Microsoft.Extensions.Caching.Memory;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers;

public class RegisterCompleteCommandHandler : IRequestHandler<RegisterCompleteCommand>
{
    private readonly IMemoryCache _cache;
    private readonly IRepository<AppUser> _repository;

    public RegisterCompleteCommandHandler(IMemoryCache cache, IRepository<AppUser> repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task Handle(RegisterCompleteCommand request, CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue(request.Email, out TempRegisterModel temp))
            throw new Exception("Kodun süresi doldu veya kayıt bulunamadı.");

        if (temp.VerificationCode != request.VerificationCode)
            throw new Exception("Kod yanlış.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(temp.Password);
        var user = new AppUser
        {
            Username = temp.Username,
            Password = passwordHash,
            Name = temp.Name,
            Surname = temp.Surname,
            Email = temp.Email,
            SubscriptionStatus = "free",
            EmailVerificationCode = null,
            IsEmailVerified = true
        };
        await _repository.CreateAsync(user);

        _cache.Remove(request.Email);
    }
}