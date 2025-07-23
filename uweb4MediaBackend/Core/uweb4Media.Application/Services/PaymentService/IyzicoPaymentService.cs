using System.Globalization;
using Iyzipay.Model; 
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Services.PaymentService
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly Iyzipay.Options _options;

        public IyzicoPaymentService(IConfiguration config)
        {
            _options = new Iyzipay.Options
            {
                ApiKey = config["Iyzico:ApiKey"],
                SecretKey = config["Iyzico:SecretKey"],
                BaseUrl = config["Iyzico:BaseUrl"]
            };
        }

        public Task<string> CreatePaymentAsync(
            decimal amount,
            string orderId,
            string email,
            string cardHolderName,
            string cardNumber,
            string expireMonth,
            string expireYear,
            string cvc)
        {
            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = orderId,
                Price = amount.ToString("0.00", CultureInfo.InvariantCulture),
                PaidPrice = amount.ToString("0.00", CultureInfo.InvariantCulture),
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = orderId,
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };

            request.PaymentCard = new PaymentCard
            {
                CardHolderName = cardHolderName,
                CardNumber = cardNumber,
                ExpireMonth = expireMonth,
                ExpireYear = expireYear,
                Cvc = cvc,
                RegisterCard = 0
            };

            request.Buyer = new Buyer
            {
                Id = "BY789",
                Name = "Onion",
                Surname = "Test",
                GsmNumber = "+905350000000",
                Email = email,
                IdentityNumber = "74300864791",
                RegistrationAddress = "Adres",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey"
            };

            request.ShippingAddress = new Address
            {
                ContactName = cardHolderName,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Ev adresi",
                ZipCode = "34000"
            };

            request.BillingAddress = new Address
            {
                ContactName = cardHolderName,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Fatura adresi",
                ZipCode = "34000"
            };

            request.BasketItems = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = "BI101",
                    Name = "Test Ürün",
                    Category1 = "Genel",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = amount.ToString("0.00", CultureInfo.InvariantCulture)
                }
            };

            var payment = Payment.Create(request, _options);

            if (payment.Status != "success")
            {
                Console.WriteLine($"Iyzico Hata: {payment.ErrorMessage}, Kod: {payment.ErrorCode}");
            }

            var result = payment.Status == "success" ? payment.PaymentId : null;
            return Task.FromResult(result);
        }
    }
}