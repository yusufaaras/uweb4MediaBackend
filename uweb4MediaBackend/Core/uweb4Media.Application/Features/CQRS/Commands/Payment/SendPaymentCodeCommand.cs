using MediatR;

namespace uweb4Media.Application.Features.CQRS.Commands.Payment;

public class SendPaymentCodeCommand : IRequest<int>
{
    public decimal Amount { get; set; }
    public string OrderId { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Currency { get; set; }
    
    public int? PlanId { get; set; }
}