using uweb4Media.Application.Features.CQRS.Queries.Subscription;
using uweb4Media.Application.Features.CQRS.Results.Subscription;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Subscription;

public class GetSubscriptionByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Subscription> _repository;
    public GetSubscriptionByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Subscription> repository)
    {
        _repository = repository;
    }
    public async Task<GetSubscriptionByIdQueryResult> Handle(GetSubscriptionByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetSubscriptionByIdQueryResult
        {
            SubscriberUserId = values.SubscriberUserId,
            AuthorUserId = values.AuthorUserId,
            SubscribedDate = values.SubscribedDate,
        };
    }
}