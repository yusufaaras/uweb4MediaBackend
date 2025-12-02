using uweb4Media.Application.Features.CQRS.Commands.Subscription;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Subscription;

public class RemoveSubscriptionCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Subscription> _repository;
    public RemoveSubscriptionCommandHandler(IRepository<Uweb4Media.Domain.Entities.Subscription> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveSubscriptionCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}