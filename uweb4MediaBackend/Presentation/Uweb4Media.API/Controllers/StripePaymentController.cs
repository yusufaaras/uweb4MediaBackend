using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces.Payment;
using Uweb4Media.Domain.Entities.StripePayment;
using System.Linq;
using Uweb4Media.Persistence.Context;
using System.Threading.Tasks;
using uweb4Media.Application.Features.CQRS.Queries.Invoice;

namespace Uweb4Media.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripePaymentController : Controller
{
    private readonly IMediator _mediator;
    private readonly IPaymentRepository _paymentRepo;
    private readonly IConfiguration _config;
    private readonly IStripeConnectService _stripeConnectService;
    private readonly Uweb4MediaContext _db;

    public StripePaymentController(
        IMediator mediator,
        IPaymentRepository paymentRepo,
        IConfiguration config,
        IStripeConnectService stripeConnectService,
        Uweb4MediaContext db)
    {
        _mediator = mediator;
        _paymentRepo = paymentRepo;
        _config = config;
        _stripeConnectService = stripeConnectService;
        _db = db;
    }

    [HttpPost("create-stripe-intent")]
    public async Task<IActionResult> CreateStripeIntent([FromBody] CreateStripePaymentIntentCommand command)
    { 
        var clientSecret = await _mediator.Send(command);
        return Ok(new { clientSecret });
    }

    [HttpPost("stripe-webhook")]
public async Task<IActionResult> StripeWebhook()
{
    string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
    Event stripeEvent;
    try
    {
        stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            _config["Stripe:WebhookSecret"]
        );
    }
    catch (Exception ex)
    {
        return BadRequest($"Webhook Error: {ex.Message}");
    }

    if (stripeEvent.Type == "payment_intent.succeeded")
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        var payment = await _paymentRepo.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
        if (payment != null)
        {
            payment.Status = "success";
            await _paymentRepo.UpdateAsync(payment);

            // Abonelik ise: premium + start/end date
            if (!payment.IsToken && payment.PlanId != null)
            {
                var user = await _db.AppUsers.FindAsync(payment.UserId);
                if (user != null)
                {
                    user.SubscriptionStatus = "premium";
                    user.SubscriptionStartDate = DateTime.UtcNow;
                    user.SubscriptionEndDate = DateTime.UtcNow.AddYears(1); // veya AddMonths(1)
                    await _db.SaveChangesAsync();
                }
            }

            // Token ise: token ekle
            if (payment.IsToken && payment.PlanId != null)
            {
                var user = await _db.AppUsers.FindAsync(payment.UserId);
                var plan = await _db.Plans.FindAsync(payment.PlanId);

                int tokenCount = 1;
                if (plan != null && plan.IsToken && plan.TokenCount.HasValue)
                    tokenCount = plan.TokenCount.Value;

                if (user != null)
                {
                    user.PostToken += tokenCount;
                    await _db.SaveChangesAsync();
                }
            }
            // Partner paylaşımları ve fatura işlemleri devam...
            var shares = _db.PartnerShares.ToList();
            await _stripeConnectService.DistributePaymentAsync(paymentIntent.Id, shares);

            // Stripe Customer oluştur veya bul
            var customerService = new CustomerService();
            var customerList = await customerService.ListAsync(new CustomerListOptions { Email = payment.Email });
            var customer = customerList.Data.FirstOrDefault();
            if (customer == null)
            {
                customer = await customerService.CreateAsync(new CustomerCreateOptions
                {
                    Email = payment.Email,
                    Name = payment.Email // Varsa isim alanını da kullanabilirsin
                });
            }

            // --- Önce Invoice oluştur ---
            var invoiceService = new InvoiceService();
            var invoice = await invoiceService.CreateAsync(new InvoiceCreateOptions
            {
                Customer = customer.Id,
                CollectionMethod = "send_invoice",
                DaysUntilDue = 0
            });

            // --- Sonra InvoiceItem'ı invoice'a bağlayarak ekle ---
            var invoiceItemService = new InvoiceItemService();
            await invoiceItemService.CreateAsync(new InvoiceItemCreateOptions
            {
                Customer = customer.Id,
                Invoice = invoice.Id,
                Amount = (long)(payment.Amount * 100),
                Currency = payment.Currency,
                Description = "Ödeme faturası"
            });

            // Invoice finalize et ve e-posta gönder
            await invoiceService.FinalizeInvoiceAsync(invoice.Id);
            var finalizedInvoice = await invoiceService.GetAsync(invoice.Id);
            if (finalizedInvoice.Status == "open")
            {
                await invoiceService.SendInvoiceAsync(finalizedInvoice.Id);
            }

            payment.InvoiceId = finalizedInvoice.Id;
            payment.InvoicePdfUrl = finalizedInvoice.InvoicePdf ?? finalizedInvoice.HostedInvoiceUrl;
            payment.InvoiceStatus = finalizedInvoice.Status;
            await _paymentRepo.UpdateAsync(payment);
        }
    }

    return Ok();
}


    [HttpGet("get-kyc-link")]
    public async Task<IActionResult> GetKycLink([FromQuery] int userId)
    {
        var user = await _db.AppUsers.FindAsync(userId);
        if (user == null || string.IsNullOrEmpty(user.StripeAccountId))
            return NotFound(new { error = "Stripe hesabı bulunamadı." });
        var url = await _stripeConnectService.CreateAccountLinkAsync(user.StripeAccountId);
        if (string.IsNullOrEmpty(url))
            return StatusCode(500, new { error = "KYC bağlantısı oluşturulamadı." });

        return Ok(new { url });
    }
    [HttpPost("request-payment-code")]
    public async Task<IActionResult> RequestPaymentCode([FromBody] SendPaymentCodeCommand command)
    {
        try
        {
            var paymentId = await _mediator.Send(command);
            return Ok(new { paymentId });
        }
        catch (Exception ex)
        {
            // Hata mesajını loglamak için:
            Console.WriteLine("RequestPaymentCode hatası: " + ex.ToString());

            // Frontend'e hata mesajı ile dön:
            return StatusCode(500, new { error = ex.Message, detail = ex.ToString() });
        }
    }

    [HttpPost("create-stripe-intent-with-code")]
    public async Task<IActionResult> CreateStripeIntentWithCode([FromBody] CreateStripeIntentWithCodeCommand command)
    {
        var clientSecret = await _mediator.Send(command);
        return Ok(new { clientSecret });
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetInvoicesByUserId(int userId)
    {
        var result = await _mediator.Send(new GetInvoicesQuery(userId));
        return Ok(result);
    }
}