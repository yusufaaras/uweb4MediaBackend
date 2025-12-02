namespace uweb4Media.Application.Features.CQRS.Commands.User;

public class SubscribeUserCommand
{
    public int AppUserID { get; set; }  
    public string? SubscriptionStatus { get; set; }
}