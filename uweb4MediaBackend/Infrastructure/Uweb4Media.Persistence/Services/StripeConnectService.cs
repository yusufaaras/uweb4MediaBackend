using Microsoft.Extensions.Configuration;
using Stripe;
using uweb4Media.Application.Interfaces.Payment;
using Uweb4Media.Domain.Entities.StripePayment;
using Uweb4Media.Persistence.Context;

namespace uweb4Media.Persistence.Services.PaymentService
{
    public class StripeConnectService : IStripeConnectService
    {
        private readonly Uweb4MediaContext _db;

        public StripeConnectService(IConfiguration config, Uweb4MediaContext db)
        {
            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
            _db = db;
        }

public async Task DistributePaymentAsync(string paymentIntentId, IEnumerable<PartnerShare> shares)
{
    if (shares == null || !shares.Any())
    {
        Console.WriteLine("PartnerShares tablosu boş veya null, dağıtım yapılmadı.");
        return;
    }

    var paymentIntentService = new PaymentIntentService();
    var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

    var chargeService = new ChargeService();
    var charges = await chargeService.ListAsync(new ChargeListOptions
    {
        PaymentIntent = paymentIntentId,
        Limit = 1
    });
    var charge = charges.Data.FirstOrDefault();
    if (charge == null)
        throw new Exception("No charge found for this PaymentIntent.");

    long stripeFee = 0;
    if (charge.BalanceTransaction != null)
    {
        stripeFee = charge.BalanceTransaction.Fee;
    }
    else if (!string.IsNullOrEmpty(charge.BalanceTransactionId))
    {
        var balanceTransactionService = new BalanceTransactionService();
        var balanceTransaction = await balanceTransactionService.GetAsync(charge.BalanceTransactionId);
        stripeFee = balanceTransaction.Fee;
    }

    // Fee alınamazsa charge.Amount ile devam et (test ortamı için)
    var distributableAmount = charge.Amount - stripeFee;
    if (stripeFee == 0)
        distributableAmount = charge.Amount;

    foreach (var share in shares)
    {
        if (string.IsNullOrEmpty(share.StripeAccountId))
        {
            Console.WriteLine($"PartnerShare id {share.Id} için StripeAccountId null veya boş, atlanıyor.");
            continue;
        }

        var partnerUser = _db.AppUsers.FirstOrDefault(x => x.StripeAccountId == share.StripeAccountId);
        if (partnerUser == null)
        {
            Console.WriteLine($"AppUser tablosunda StripeAccountId {share.StripeAccountId} bulunamadı, atlanıyor.");
            continue;
        }
        if (!partnerUser.IsKYCVerified)
        {
            Console.WriteLine($"AppUser id {partnerUser.AppUserID} KYC doğrulanmamış, atlanıyor.");
            continue;
        }

        var amount = (long)(distributableAmount * (share.Percentage / 100));
        if (amount <= 0)
        {
            Console.WriteLine($"Dağıtılacak tutar 0 veya altı ({amount}), atlanıyor.");
            continue;
        }

        var transferService = new TransferService();
        try
        {
            await transferService.CreateAsync(new TransferCreateOptions
            {
                Amount = amount,
                Currency = paymentIntent.Currency,
                Destination = share.StripeAccountId,
                SourceTransaction = charge.Id
            });
            Console.WriteLine($"Transfer başarıyla gönderildi: {amount} {paymentIntent.Currency} hesabına {share.StripeAccountId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transfer sırasında hata oluştu: {ex.Message}");
        }
    }
}
        public async Task<string> CreateAccountLinkAsync(string stripeAccountId)
        {
            var options = new Stripe.AccountLinkCreateOptions
            {
                Account = stripeAccountId,
                RefreshUrl = "http://localhost:5173/#/subscription",
                ReturnUrl = "http://localhost:5173/#/subscription",
                Type = "account_onboarding",
            };
            var service = new Stripe.AccountLinkService();
            var link = await service.CreateAsync(options);
            return link.Url;
        }
    }
}