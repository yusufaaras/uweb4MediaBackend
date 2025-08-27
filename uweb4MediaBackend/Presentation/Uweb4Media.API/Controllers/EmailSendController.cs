using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Interfaces.Email;
using uweb4Media.Application.Services.Email;

namespace Uweb4Media.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IVerificationService _verificationService;

    public VerificationController(IEmailService emailService, IVerificationService verificationService)
    {
        _emailService = emailService;
        _verificationService = verificationService;
    }

    [HttpPost("send-code")]
    [Authorize]  
    public async Task<IActionResult> SendCode()
    {
        var userEmail = User.Claims.FirstOrDefault(c =>
            c.Type == "email" ||
            c.Type == "emailaddress" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        )?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return BadRequest("Email bulunamadı!");

        var code = _verificationService.GenerateAndSaveCode(userEmail); 
        var subject = "Onay Kodunuz";
        var body = $"Kodunuz: {code}";

        await _emailService.SendEmailAsync(userEmail, subject, body);

        return Ok(new { message = "Kod gönderildi." });
    }

    [HttpPost("verify-code")]
    [Authorize]
    public async Task<IActionResult> VerifyCode([FromBody] VerificationRequest req)
    {
        var userEmail = User.Claims.FirstOrDefault(c =>
            c.Type == "email" ||
            c.Type == "emailaddress" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        )?.Value;

        if (string.IsNullOrEmpty(userEmail))
            return BadRequest("Email bulunamadı!");

        var valid = _verificationService.VerifyCode(userEmail, req.Code);
        if (!valid)
            return BadRequest("Kod yanlış veya süresi doldu!");

        return Ok(new { valid = true });
    }
}

public class VerificationRequest
{
    public string Code { get; set; }
}