namespace uweb4Media.Application.Features.CQRS.Commands.Notification;

public class CreateNotificationCommand
{
    public int UserId { get; set; }
    public string Text { get; set; }
}