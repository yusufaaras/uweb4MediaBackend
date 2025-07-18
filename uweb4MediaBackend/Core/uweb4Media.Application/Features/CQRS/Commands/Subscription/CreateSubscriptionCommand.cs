namespace uweb4Media.Application.Features.CQRS.Commands.Subscription;

public class CreateSubscriptionCommand
{
    public int SubscriberUserId { get; set; } // Abone olan kullanıcı ID'si
    public int AuthorUserId { get; set; } 
}