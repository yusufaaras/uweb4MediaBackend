using Microsoft.Extensions.Configuration;
using Stripe;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Services.PaymentService;

public class StripePaymentService : IStripePaymentService
{
    private readonly IConfiguration _config;

    public StripePaymentService(IConfiguration config)
    {
        _config = config;
        Stripe.StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }
 
    public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
        decimal amount, 
        string orderId, 
        string email, 
        string currency  
    )
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = currency ?? "usd",  
            Metadata = new Dictionary<string, string>
            {
                { "orderId", orderId },
                { "email", email }
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return (intent.Id, intent.ClientSecret);
    }
}