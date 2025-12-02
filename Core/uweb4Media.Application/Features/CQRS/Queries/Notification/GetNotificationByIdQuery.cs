namespace uweb4Media.Application.Features.CQRS.Queries.Notification;

public class GetNotificationByIdQuery
{
    public GetNotificationByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}