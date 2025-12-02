namespace uweb4Media.Application.Features.CQRS.Results.Notification;

public class GetNotificationQueryResult
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public string Message { get; set; } 
    public string Type { get; set; } 
    public bool IsRead { get; set; } 
    public DateTime NotificationDate { get; set; }
    public string Time { get; set; } 
}