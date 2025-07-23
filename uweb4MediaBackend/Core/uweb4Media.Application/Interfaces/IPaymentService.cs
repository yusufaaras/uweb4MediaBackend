namespace uweb4Media.Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(
        decimal amount,
        string orderId,
        string email,
        string cardHolderName,
        string cardNumber,
        string expireMonth,
        string expireYear,
        string cvc
    );
}