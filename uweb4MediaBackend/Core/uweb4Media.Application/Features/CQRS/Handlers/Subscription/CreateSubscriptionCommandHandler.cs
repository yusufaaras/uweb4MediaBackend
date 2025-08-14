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
        // Validasyon: Sadece biri dolu olmalı!
        if ((command.AuthorUserId == null && command.AuthorCompanyId == null) ||
            (command.AuthorUserId != null && command.AuthorCompanyId != null))
        {
            throw new Exception("Takip edilen sadece bir kullanıcı veya bir şirket olmalı.");
        }

        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Subscription
        {
            SubscriberUserId = command.SubscriberUserId,
            AuthorUserId = command.AuthorUserId,
            AuthorCompanyId = command.AuthorCompanyId
        });
    }
}