using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Tools;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrosoftAuthController : ControllerBase
    {
        private readonly IAppUserRepository _appUserRepository;

        public MicrosoftAuthController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        [HttpGet("login")]
        public IActionResult MicrosoftLogin()
        {
            var redirectUri = $"{Request.Scheme}://{Request.Host}/api/MicrosoftAuth/callback";
            var props = new AuthenticationProperties { RedirectUri = redirectUri };
            return Challenge(props, "Microsoft");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> MicrosoftCallback()
        {
            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var microsoftId = claims.FirstOrDefault(c => c.Type == "MicrosoftId")?.Value;
            var microsoftName = claims.FirstOrDefault(c => c.Type == "MicrosoftName")?.Value;

            // Kullanıcıyı getir/yoksa oluştur
            var user = await _appUserRepository.GetByMicrosoftIdAsync(microsoftId);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = email,
                    MicrosoftId = microsoftId,
                    Username = microsoftName ?? email,
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
            return Redirect("https://prime.uweb4.com/auth/microsoft-success?token=" + token.Token);        
        }
    }
}