using Microsoft.Extensions.Caching.Memory;

namespace uweb4Media.Application.Services.Email;

public class VerificationService : IVerificationService
{
    private readonly IMemoryCache _cache;

    public VerificationService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public string GenerateAndSaveCode(string email)
    {
        var code = new Random().Next(100000, 999999).ToString();
        _cache.Set("verify_" + email, code, TimeSpan.FromMinutes(10));
        return code;
    }

    public bool VerifyCode(string email, string code)
    {
        if (_cache.TryGetValue("verify_" + email, out string stored) && stored == code)
        {
            _cache.Remove("verify_" + email);
            return true;
        }
        return false;
    }
}

public interface IVerificationService
{
    string GenerateAndSaveCode(string email);
    bool VerifyCode(string email, string code);
}