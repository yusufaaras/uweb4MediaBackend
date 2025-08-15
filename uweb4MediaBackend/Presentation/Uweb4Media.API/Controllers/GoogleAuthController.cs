using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Commands.GoogleCommands;
using uweb4Media.Application.Features.Mediator.Queries.GoogleQueries;
using uweb4Media.Application.Tools;

namespace Uweb4Media.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator; // IMediator eklendi

    public AuthController(IConfiguration configuration, IMediator mediator) // Constructor güncellendi
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        // React uygulamasından doğrudan bu endpoint'e yönlendirilecek.
        // Google'ın kendi giriş sayfasını tetikler.
        var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleCallback)) };
        return Challenge(properties, "Google"); // "Google" stringi, AddGoogle metodunda tanımladığınız default şemadır.
    }

    [HttpGet("google-callback")]
public async Task<IActionResult> GoogleCallback()
{
    try
    {
        var authenticateResult = await HttpContext.AuthenticateAsync("Google");

        if (!authenticateResult.Succeeded)
        {
            return Redirect($"{_configuration["Jwt:Audience"]}/login-error?message=Google kimlik doğrulama başarısız.");
        }

        var claims = authenticateResult.Principal.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var surname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var profilePic = claims.FirstOrDefault(c => c.Type == "picture")?.Value;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(googleId))
        {
            return Redirect($"{_configuration["Jwt:Audience"]}/login-error?message=Google'dan gerekli bilgiler alınamadı.");
        }

        var createGoogleUserCommand = new CreateGoogleAppUserCommand
        {
            Email = email,
            Name = name,
            Surname = surname,
            GoogleId = googleId,
            AvatarUrl = profilePic
        };
        await _mediator.Send(createGoogleUserCommand);

        var userCheckQuery = new GetAppUserByGoogleIdQuery { GoogleId = googleId, Email = email };
        var userResult = await _mediator.Send(userCheckQuery);

        if (!userResult.IsExits)
        {
            return Redirect($"{_configuration["Jwt:Audience"]}/login-error?message=Kullanıcı bilgisi veritabanında bulunamadı.");
        }

        var tokenDto = JwtTokenGenerator.GenerateToken(userResult);
        var redirectUrl = $"{_configuration["Jwt:Audience"]}/auth/google-success" +
                          $"?token={tokenDto.Token}" +
                          $"&email={email}" +
                          $"&name={name}" +
                          $"&profilePic={profilePic}" +
                          $"&expires={tokenDto.ExpireDate.ToString("o")}";

        return Redirect(redirectUrl);
    }
    catch (Exception ex)
    {
        // Hata detayını browser'da görebilmek için:
        return Content($"EXCEPTION: {ex.Message} <br/><br/> {ex.StackTrace}");
    }
}
    
    [HttpGet("protected-data")]
    public IActionResult GetProtectedData()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok($"Merhaba {userName ?? userEmail}, bu korumalı bir veri!");
    }
}