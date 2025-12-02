namespace uweb4Media.Application.Helper;

public static class EmailDomainHelper
{
    // Allowlist kullanmak istersen:
    public static readonly List<string> AllowedDomains = new()
    {
        "gmail.com", "outlook.com", "hotmail.com", "company.com"
    };

    // Blacklist kullanmak istersen:
    public static readonly List<string> BlacklistDomains = new()
    {
        "tempmail.com", "mailinator.com", "10minutemail.com", "guerrillamail.com"
    }; 
    public static bool IsAllowedDomain(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@")) return false;
        var domain = email.Split('@')[1].ToLowerInvariant(); 
        return !BlacklistDomains.Any(d => domain == d || domain.EndsWith("." + d));
    }
}