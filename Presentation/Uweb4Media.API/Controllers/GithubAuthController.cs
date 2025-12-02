using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Tools;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GitHubAuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppUserRepository _appUserRepository;

    public GitHubAuthController(IMediator mediator, IAppUserRepository appUserRepository)
    {
        _mediator = mediator;
        _appUserRepository = appUserRepository;
    }

    [HttpGet("login")]
    public IActionResult GitHubLogin()
    {
        var redirectUri = $"{Request.Scheme}://{Request.Host}/api/GitHubAuth/callback";
        var props = new AuthenticationProperties { RedirectUri = redirectUri };
        return Challenge(props, "GitHub");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> GitHubCallback()
    {
        var result = await HttpContext.AuthenticateAsync("External");
        if (!result.Succeeded)
            return Unauthorized();

        var claims = result.Principal.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var githubId = claims.FirstOrDefault(c => c.Type == "GithubId")?.Value;
        var githubLogin = claims.FirstOrDefault(c => c.Type == "GithubLogin")?.Value;
        var avatarUrl = claims.FirstOrDefault(c => c.Type == "AvatarUrl")?.Value;

        // Kullanıcıyı getir/yoksa oluştur
        var user = await _appUserRepository.GetByGithubIdAsync(githubId);
        if (user == null)
        {
            user = new AppUser
            {
                Email = email,
                GithubId = githubId,
                Username = githubLogin ?? email,
                AvatarUrl = avatarUrl,
                SubscriptionStatus = "free",
                AppRoleID = 2, // veya otomatik admin yapmak istiyorsan 1
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
        return Redirect("https://prime.uweb4.com/auth/github-success?token=" + token.Token);        
    }
}