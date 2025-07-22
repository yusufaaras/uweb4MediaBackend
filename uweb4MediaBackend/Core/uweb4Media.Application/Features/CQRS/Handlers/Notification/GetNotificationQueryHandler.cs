using MediatR;
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
    public async Task<List<GetNotificationQueryResult>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    { 
        var values = await _repository.GetAllAsync();

        return values.Select(x => new GetNotificationQueryResult
        {
            Id = x.Id,
            UserId = x.UserId,
            Message = x.Message,  
            Type = x.Type,
            IsRead = x.IsRead,
            NotificationDate = x.NotificationDate,
            Time = FormatNotificationTime(x.NotificationDate)  
        }).ToList();
    }

    private string FormatNotificationTime(DateTime notificationDate)
    {
        var timeSince = DateTime.UtcNow - notificationDate;

        if (timeSince.TotalMinutes < 1) return "Şimdi";
        if (timeSince.TotalMinutes < 60) return $"{Math.Floor(timeSince.TotalMinutes)} dakika önce";
        if (timeSince.TotalHours < 24) return $"{Math.Floor(timeSince.TotalHours)} saat önce";
        if (timeSince.TotalDays < 7) return $"{Math.Floor(timeSince.TotalDays)} gün önce";

        // Yıl, ay, gün ve saat formatı (örn: 22 Temmuz 2025 13:04)
        return notificationDate.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR"));
    }
}

public class GetNotificationQuery : IRequest<List<GetNotificationQueryResult>>
{ 
}