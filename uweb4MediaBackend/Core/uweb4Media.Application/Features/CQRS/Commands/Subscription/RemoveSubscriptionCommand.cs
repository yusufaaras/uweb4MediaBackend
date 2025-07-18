namespace uweb4Media.Application.Features.CQRS.Commands.Subscription;

public class RemoveSubscriptionCommand
{
    public  RemoveSubscriptionCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}