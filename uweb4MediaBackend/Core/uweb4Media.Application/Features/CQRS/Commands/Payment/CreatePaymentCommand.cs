using MediatR;

namespace uweb4Media.Application.Features.CQRS.Commands.Payment;

public class CreatePaymentCommand : IRequest<string>
{
    public decimal Amount { get; set; }
    public string OrderId { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; } 
    public string CardHolderName { get; set; }
    
    public bool IsToken { get; set; } 
    public string CardNumber { get; set; }
    public string ExpireMonth { get; set; }
    public string ExpireYear { get; set; }
    public string Cvc { get; set; }
}