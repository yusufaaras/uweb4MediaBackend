using MediatR;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers.AppUserHandlers;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IRepository<AppUser> _repository;
    public VerifyEmailCommandHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByFilterAsync(x => x.Email == request.Email);

        if (user == null || user.IsEmailVerified)
            return false;

        if (user.EmailVerificationCode == request.VerificationCode)
        {
            user.IsEmailVerified = true;
            user.EmailVerificationCode = null;
            await _repository.UpdateAsync(user);
            return true;
        }
        return false;
    }
}