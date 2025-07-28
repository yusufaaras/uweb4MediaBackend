using System.Net;
using System.Net.Mail;
using uweb4Media.Application.Interfaces.Email;

namespace uweb4Media.Application.Services.Email;

public class SmtpEmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("username", "password"),
            EnableSsl = true,
        };
        var mail = new MailMessage("from@example.com", to, subject, body);
        await client.SendMailAsync(mail);
    }
}