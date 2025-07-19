using uweb4Media.Application.Features.CQRS.Queries.User;
using uweb4Media.Application.Features.CQRS.Results.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class GetSubscribeUserByIdQueryHandler
{
    private readonly IRepository<AppUser> _repository;

    public GetSubscribeUserByIdQueryHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task<GetSubscribeUserByIdQueryResult> Handle(GetSubscribeUserByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetSubscribeUserByIdQueryResult
        {
            AppUserID = values.AppUserID, 
            SubscriptionStatus = values.SubscriptionStatus,
        };
    }
}