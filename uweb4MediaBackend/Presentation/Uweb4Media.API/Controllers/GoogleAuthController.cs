using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Commands.GoogleCommands;
using uweb4Media.Application.Features.Mediator.Queries.GoogleQueries;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Tools;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoogleAuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppUserRepository _appUserRepository;

    public GoogleAuthController(IMediator mediator, IAppUserRepository appUserRepository)
    {
        _mediator = mediator;
        _appUserRepository = appUserRepository;
    }

    [HttpGet("login")]
    public IActionResult GoogleLogin()
    {
        var redirectUri = $"{Request.Scheme}://{Request.Host}/api/GoogleAuth/callback";
        var props = new AuthenticationProperties { RedirectUri = redirectUri };
        return Challenge(props, "Google");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync("External");
        if (!result.Succeeded)
            return Unauthorized();

        var claims = result.Principal.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        var surname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        var avatarUrl = claims.FirstOrDefault(c => c.Type == "profile_picture")?.Value;

        // Kullanıcıyı getir/yoksa oluştur
        var user = await _appUserRepository.GetByGoogleIdAsync(googleId);
        if (user == null)
        {
            user = new AppUser
            {
                Email = email,
                GoogleId = googleId,
                Name = name,
                Surname = surname,
                Username = email,
                AvatarUrl = avatarUrl,
                SubscriptionStatus = "free",
                AppRoleID = 2,
                IsEmailVerified = true
            };
            await _appUserRepository.AddAsync(user);
        }

        var appUserResult = new GetCheckAppUserQueryResult
        {
            IsExits = true,
            AppUserID = user.AppUserID,
            Username = user.Username,
            Email = user.Email,
            Role = user.AppRole?.Name,
            Name = user.Name,
            Surname = user.Surname,
            AvatarUrl = user.AvatarUrl,
            GoogleId = user.GoogleId
        };
        var token = JwtTokenGenerator.GenerateToken(appUserResult);
        return Redirect("https://prime.uweb4.com/auth/google-success?token=" + token.Token);
    }
}