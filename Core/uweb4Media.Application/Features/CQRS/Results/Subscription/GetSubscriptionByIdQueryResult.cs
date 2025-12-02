namespace uweb4Media.Application.Features.CQRS.Results.Subscription;

public class GetSubscriptionByIdQueryResult
{
    public int SubscriberUserId { get; set; } 

    // Takip edilen kullanıcı veya şirket
    public int? AuthorUserId { get; set; }
    public int? AuthorCompanyId { get; set; }

    public DateTime SubscribedDate { get; set; }
}