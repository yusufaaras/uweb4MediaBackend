using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Features.CQRS.Commands.Subscription;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;

public class CreateSubscriptionCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Subscription> _repository;

    public CreateSubscriptionCommandHandler(IRepository<Uweb4Media.Domain.Entities.Subscription> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateSubscriptionCommand command)
    {
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Subscription
        { 
            SubscriberUserId = command.SubscriberUserId,
            AuthorUserId=command.AuthorUserId
        });
    }
}