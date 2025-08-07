using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using uweb4Media.Application.Interfaces.Payment;

namespace uweb4Media.Application.Services.PaymentService;

public class StripePaymentService : IStripePaymentService
{
    private readonly IConfiguration _config;

    public StripePaymentService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
        decimal amount,
        string orderId,
        string email,
        string currency
    )
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config["Stripe:SecretKey"]);

        var parameters = new Dictionary<string, string>
        {
            { "amount", ((long)(amount * 100)).ToString() },
            { "currency", currency ?? "usd" },
            { "automatic_tax[enabled]", "true" },
            { "metadata[orderId]", orderId },
            { "metadata[email]", email }
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await httpClient.PostAsync("https://api.stripe.com/v1/payment_intents", content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Stripe PaymentIntent oluþturulamadý: {responseString}");
        }

        // Yanýtý parse et
        var json = JObject.Parse(responseString);
        var intentId = json["id"]?.ToString();
        var clientSecret = json["client_secret"]?.ToString();

        if (string.IsNullOrEmpty(intentId) || string.IsNullOrEmpty(clientSecret))
            throw new Exception("Stripe yanýtý beklenmedik formatta.");

        return (intentId, clientSecret);
    }
}