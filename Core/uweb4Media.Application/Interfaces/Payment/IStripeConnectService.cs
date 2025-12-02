using Uweb4Media.Domain.Entities.StripePayment;

namespace uweb4Media.Application.Interfaces.Payment;

public interface IStripeConnectService
{
    Task DistributePaymentAsync(string paymentIntentId, IEnumerable<PartnerShare> shares);

    Task<string> CreateAccountLinkAsync(string stripeAccountId);
}