namespace CarSales.Web.Services;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);
}
