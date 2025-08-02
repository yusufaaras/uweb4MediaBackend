using System.Net;
using System.Net.Mail;
using uweb4Media.Application.Interfaces.Email;

namespace uweb4Media.Application.Services.Email;

public class SmtpEmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient("server327.web-hosting.com")
        {
            Port = 587, // 587 kullan! (STARTTLS)
            Credentials = new NetworkCredential("info@unitedweb4.com", "Uweb4.2025"),
            EnableSsl = true,
            Timeout = 10000 // 10 saniye
        };
        var mail = new MailMessage("info@unitedweb4.com", to, subject, body);

        try
        {
            await client.SendMailAsync(mail);
        }
        catch(Exception ex)
        {
            // Hata logla, gerekirse dışarı fırlat
            Console.WriteLine("Mail gönderme hatası: " + ex.Message);
            throw new Exception("Mail gönderme başarısız: " + ex.Message, ex);
        }
    }
}