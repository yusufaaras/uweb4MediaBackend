namespace uweb4Media.Application.Interfaces.Payment;

public interface IStripePaymentService
{ 
    Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(decimal amount, string orderId, string email);
}