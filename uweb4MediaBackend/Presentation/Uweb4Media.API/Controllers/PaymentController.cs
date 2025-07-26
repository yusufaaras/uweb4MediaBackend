using System.Globalization;
using Iyzipay.Model;
using Uweb4Media.Domain.Entities;
using Iyzipay.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Payment;
using uweb4Media.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using uweb4Media.Application.Features.CQRS.Handlers.Payment;
using uweb4Media.Application.Features.CQRS.Queries.Payment;
using Payment = Uweb4Media.Domain.Entities.Payment;

namespace Uweb4Media.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<AppUser> _userRepo;
        private readonly GetPaymentsByUserIdQueryHandler _getPaymentsByUserIdQueryHandler;

        public PaymentController(IMediator mediator, IConfiguration config, IRepository<Payment> paymentRepo, IRepository<AppUser> userRepo, GetPaymentsByUserIdQueryHandler getPaymentsByUserIdQueryHandler)
        {
            _mediator = mediator;
            _config = config;
            _paymentRepo = paymentRepo;
            _userRepo = userRepo;
            _getPaymentsByUserIdQueryHandler = getPaymentsByUserIdQueryHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
        {
            try
            {
                var paymentId = await _mediator.Send(command);
                return Ok(new { paymentId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback()
        {
            
            var token = Request.Form["token"].FirstOrDefault();
            Console.WriteLine($"Callback çalıştı! Token: {token}");

            var options = new Iyzipay.Options
            {
                ApiKey = _config["Iyzico:ApiKey"],
                SecretKey = _config["Iyzico:SecretKey"],
                BaseUrl = _config["Iyzico:BaseUrl"]
            };

            var request = new RetrieveCheckoutFormRequest
            {
                Locale = "tr",
                Token = token 
            };

            var result = CheckoutForm.Retrieve(request, options); 
            var orderId = result.BasketId ?? result.ConversationId;

            //Console.WriteLine($"Iyzipay callback result: {System.Text.Json.JsonSerializer.Serialize(result)}");
           //Console.WriteLine($"Callback gerçek orderId: {orderId}, token: {token}");

            if (result.PaymentStatus == "SUCCESS")
            { 
                var payment = await _paymentRepo.GetByFilterAsync(p => p.OrderId == orderId);
                if (payment != null)
                {
                    payment.Status = "success";
                    payment.IyzicoPaymentId = result.PaymentId;
                    await _paymentRepo.UpdateAsync(payment);

                    var user = await _userRepo.GetByIdAsync(payment.UserId);
                    if (user != null)
                    {
                        user.SubscriptionStatus = "premium";
                        await _userRepo.UpdateAsync(user);
                    }
                }
            }
            else
            {
                var payment = await _paymentRepo.GetByFilterAsync(p => p.OrderId == orderId);
                if (payment != null)
                {
                    payment.Status = "failure";
                    await _paymentRepo.UpdateAsync(payment);
                }
            }

            return Ok();
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            var values = await _getPaymentsByUserIdQueryHandler.Handle(new GetPaymentsByUserIdQuery(userId));
            return Ok(values);
        }
        
        [HttpPost("initiate")]
        public async Task<IActionResult> Initiate([FromBody] PaymentInitiateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("ModelState Errors: " + string.Join(" | ", errors));
                return BadRequest(ModelState);
            }
            if (dto.UserId <= 0 || string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Surname) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                dto.Amount <= 0)
            {
                return BadRequest("Eksik veya hatalı alan var!");
            }

            var orderId = Guid.NewGuid().ToString();

            var request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = orderId,
                Price = dto.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                PaidPrice = dto.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                Currency = "TRY",
                BasketId = orderId,
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = _config["Iyzico:CallbackUrl"], // iyzico panelinde tanımlı
                Buyer = new Buyer
                {
                    Id = dto.UserId.ToString(),
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    GsmNumber = dto.Phone ?? "+905350000000",
                    RegistrationAddress = "Adres",
                    Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    City = "Istanbul",
                    Country = "Turkey",
                    IdentityNumber = "74300864791"
                },
                BillingAddress = new Address
                {
                    ContactName = $"{dto.Name} {dto.Surname}",
                    City = "Istanbul",
                    Country = "Turkey",
                    Description = "Fatura adresi",
                    ZipCode = "34000"
                },
                ShippingAddress = new Address
                {
                    ContactName = $"{dto.Name} {dto.Surname}",
                    City = "Istanbul",
                    Country = "Turkey",
                    Description = "Teslimat adresi",
                    ZipCode = "34000"
                },
                BasketItems = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = "BI101",
                        Name = "Premium Abonelik",
                        Category1 = "Abonelik",
                        ItemType = BasketItemType.VIRTUAL.ToString(),
                        Price = dto.Amount.ToString("0.00", CultureInfo.InvariantCulture)
                    }
                }
            };

            var options = new Iyzipay.Options
            {
                ApiKey = _config["Iyzico:ApiKey"],
                SecretKey = _config["Iyzico:SecretKey"],
                BaseUrl = _config["Iyzico:BaseUrl"]
            };

            var init = CheckoutFormInitialize.Create(request, options);

            if (init.Status != "success") return BadRequest(init.ErrorMessage);

            await _paymentRepo.CreateAsync(new Payment {
                OrderId = orderId,
                Amount = dto.Amount,
                Currency = "TRY",
                Status = "pending",
                Email = dto.Email,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            });

            return Ok(new { checkoutFormContent = init.CheckoutFormContent });
        }
    }
}

public class PaymentInitiateDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
}