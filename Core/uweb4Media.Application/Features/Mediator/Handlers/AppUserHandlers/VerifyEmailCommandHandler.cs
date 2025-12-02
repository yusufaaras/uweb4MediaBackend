using MediatR;
using Microsoft.Extensions.Caching.Memory;
using uweb4Media.Application.Enums;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Model;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers.AppUserHandlers;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IMemoryCache _cache;
    private readonly IRepository<AppUser> _repository;

    public VerifyEmailCommandHandler(IMemoryCache cache, IRepository<AppUser> repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue(request.Email, out TempRegisterModel temp))
            return false;

        if (temp.VerificationCode != request.VerificationCode)
            return false;

        // Kullanıcıyı DB'ye kaydet
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(temp.Password);
        var user = new AppUser
        {
            Username = temp.Username,
            Password = passwordHash,
            AppRoleID = (int)RolesType.Member,
            Name = temp.Name,
            Surname = temp.Surname,
            Email = temp.Email,
            SubscriptionStatus = "free",
            PostToken = 0,
            EmailVerificationCode = null,
            IsEmailVerified = true
        };
        await _repository.CreateAsync(user);

        _cache.Remove(request.Email);
        return true;
    }
}