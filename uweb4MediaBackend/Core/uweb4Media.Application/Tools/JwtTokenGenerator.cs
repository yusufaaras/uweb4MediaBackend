// uweb4Media.Application.Tools/JwtTokenGenerator.cs
using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using uweb4Media.Application.Dtos;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;

namespace uweb4Media.Application.Tools
{
    public class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(GetCheckAppUserQueryResult result)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(result.Role))
                claims.Add(new Claim(ClaimTypes.Role, result.Role));

            claims.Add(new Claim(ClaimTypes.NameIdentifier, result.AppUserID.ToString()));

            if (!string.IsNullOrWhiteSpace(result.Username))
                claims.Add(new Claim("Username", result.Username));

            // Yeni eklenen alanları buraya ekleyin:
            if (!string.IsNullOrWhiteSpace(result.Email))
                claims.Add(new Claim(ClaimTypes.Email, result.Email));

            if (!string.IsNullOrWhiteSpace(result.Name))
                claims.Add(new Claim(ClaimTypes.GivenName, result.Name)); // İlk isim

            if (!string.IsNullOrWhiteSpace(result.Surname))
                claims.Add(new Claim(ClaimTypes.Surname, result.Surname)); // Soyad

            // Tam adı da ekleyebilirsiniz (opsiyonel)
            if (!string.IsNullOrWhiteSpace(result.Name) || !string.IsNullOrWhiteSpace(result.Surname))
            {
                claims.Add(new Claim(ClaimTypes.Name, $"{result.Name} {result.Surname}".Trim()));
            }

            if (!string.IsNullOrWhiteSpace(result.GoogleId))
                claims.Add(new Claim("GoogleId", result.GoogleId));

            if (!string.IsNullOrWhiteSpace(result.AvatarUrl))
                claims.Add(new Claim("AvatarUrl", result.AvatarUrl));

            // Diğer orijinal claim'leriniz:
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var singninCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // JwtTokenDefaults.Expire değerinin nasıl tanımlandığına bağlı olarak AddHours veya AddDays kullanın
            var expireDate = DateTime.UtcNow.AddHours(JwtTokenDefaults.Expire); // Varsayım: Expire saat cinsinden

            JwtSecurityToken token = new JwtSecurityToken(issuer: JwtTokenDefaults.ValidIssuer,
                                                          audience: JwtTokenDefaults.ValidAudience,
                                                          claims: claims,
                                                          notBefore: DateTime.UtcNow,
                                                          expires: expireDate,
                                                          signingCredentials: singninCredentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return new TokenResponseDto(tokenHandler.WriteToken(token), expireDate);
        }
    }
}