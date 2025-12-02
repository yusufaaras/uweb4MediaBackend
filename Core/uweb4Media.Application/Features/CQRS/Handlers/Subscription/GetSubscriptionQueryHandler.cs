using uweb4Media.Application.Features.CQRS.Results.Subscription;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Subscription;

public class GetSubscriptionQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Subscription> _repository;

    public GetSubscriptionQueryHandler(IRepository<Uweb4Media.Domain.Entities.Subscription> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetSubscriptionQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetSubscriptionQueryResult
        {
            Id = x.Id,
            SubscriberUserId = x.SubscriberUserId,
            AuthorUserId = x.AuthorUserId,
            AuthorCompanyId = x.AuthorCompanyId,
            SubscribedDate = x.SubscribedDate
        }).ToList();
    }
}