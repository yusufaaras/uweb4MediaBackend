using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class UpdateSubscribeUserCommandHandler
{
    private readonly IRepository<AppUser> _repository;
    public UpdateSubscribeUserCommandHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task Handle(SubscribeUserCommand command)
    {
        var values = await _repository.GetByIdAsync(command.AppUserID); 
        values.SubscriptionStatus = command.SubscriptionStatus;
        await _repository.UpdateAsync(values);
    }
}