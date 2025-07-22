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
    public async Task<GetNotificationByIdQueryResult> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken)
    {
        var values = await _repository.GetByIdAsync(query.Id);

        if (values == null)
        {
            return null;  
        }

        return new GetNotificationByIdQueryResult
        {
            Id = values.Id,
            UserId = values.UserId,
            Message = values.Message,  
            Type = values.Type,
            IsRead = values.IsRead,
            NotificationDate = values.NotificationDate,
            Time = FormatNotificationTime(values.NotificationDate)  
        };
    }

    private string FormatNotificationTime(DateTime notificationDate)
    {
        var timeSince = DateTime.UtcNow - notificationDate;

        if (timeSince.TotalMinutes < 1) return "Şimdi";
        if (timeSince.TotalMinutes < 60) return $"{Math.Floor(timeSince.TotalMinutes)} dakika önce";
        if (timeSince.TotalHours < 24) return $"{Math.Floor(timeSince.TotalHours)} saat önce";
        if (timeSince.TotalDays < 7) return $"{Math.Floor(timeSince.TotalDays)} gün önce"; 
        return notificationDate.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR"));
    }
}