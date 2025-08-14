namespace uweb4Media.Application.Features.CQRS.Results.Subscription;

public class GetSubscriptionQueryResult
{
    public int Id { get; set; }
    public int SubscriberUserId { get; set; }  
    public int? AuthorUserId { get; set; }
    public int? AuthorCompanyId { get; set; }

    public DateTime SubscribedDate { get; set; }
}