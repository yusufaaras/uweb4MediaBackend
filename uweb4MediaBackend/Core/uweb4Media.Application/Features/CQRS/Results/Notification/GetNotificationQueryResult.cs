namespace uweb4Media.Application.Features.CQRS.Results.Notification;

public class GetNotificationQueryResult
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public DateTime NotificationDate { get; set; }
}