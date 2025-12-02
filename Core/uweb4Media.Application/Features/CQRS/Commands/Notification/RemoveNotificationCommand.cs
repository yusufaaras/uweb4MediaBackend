namespace uweb4Media.Application.Features.CQRS.Commands.Notification;

public class RemoveNotificationCommand
{
    public  RemoveNotificationCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}