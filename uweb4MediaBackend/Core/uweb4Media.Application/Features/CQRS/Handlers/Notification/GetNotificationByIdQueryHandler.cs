using uweb4Media.Application.Features.CQRS.Queries.Notification;
using uweb4Media.Application.Features.CQRS.Results.Notification;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class GetNotificationByIdQueryHandler
{
    private readonly IRepository<Notification> _repository;
    public GetNotificationByIdQueryHandler(IRepository<Notification> repository)
    {
        _repository = repository;
    }
    public async Task<GetNotificationByIdQueryResult> Handle(GetNotificationByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetNotificationByIdQueryResult
        {
            Id = values.Id,
            UserId = values.UserId,
            Text = values.Text, 
            NotificationDate = values.NotificationDate
        };
    }
}