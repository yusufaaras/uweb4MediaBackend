using uweb4Media.Application.Features.CQRS.Results.Notification;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class GetNotificationQueryHandler
{
    private readonly IRepository<Notification> _repository;

    public GetNotificationQueryHandler(IRepository<Notification> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetNotificationQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetNotificationQueryResult
        {
            Id=x.Id,
            UserId = x.UserId,
            NotificationDate = x.NotificationDate,
            Text = x.Text,
        }).ToList();
    }
}