namespace uweb4Media.Application.Features.CQRS.Commands.Subscription;

public class CreateSubscriptionCommand
{
    public int SubscriberUserId { get; set; } 
    public int? AuthorUserId { get; set; }
 
    public int? AuthorCompanyId { get; set; }
}