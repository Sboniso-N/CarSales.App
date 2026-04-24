using System.Net;
using System.Net.Mail;
using CarSales.Web.Models.Configuration;
using Microsoft.Extensions.Options;

namespace CarSales.Web.Services;

public class SmtpEmailService(
    IOptions<EmailSettings> emailOptions,
    ILogger<SmtpEmailService> logger) : IEmailService
{
    private readonly EmailSettings settings = emailOptions.Value;

    public async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        if (!settings.Enabled || string.IsNullOrWhiteSpace(settings.SmtpHost))
        {
            logger.LogInformation(
                "Email sending skipped because SMTP is not configured. Intended recipient: {Recipient}, subject: {Subject}",
                toEmail,
                subject);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(settings.SenderEmail, settings.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        using var client = new SmtpClient(settings.SmtpHost, settings.Port)
        {
            EnableSsl = settings.UseSsl,
            Credentials = string.IsNullOrWhiteSpace(settings.Username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(settings.Username, settings.Password)
        };

        await client.SendMailAsync(message);
    }
}
