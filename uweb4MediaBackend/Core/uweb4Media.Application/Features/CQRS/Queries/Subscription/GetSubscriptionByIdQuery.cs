namespace uweb4Media.Application.Features.CQRS.Queries.Subscription;

public class GetSubscriptionByIdQuery
{
    public GetSubscriptionByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}