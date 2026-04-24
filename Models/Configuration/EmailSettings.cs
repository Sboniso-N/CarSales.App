namespace CarSales.Web.Models.Configuration;

public class EmailSettings
{
    public const string SectionName = "Email";

    public bool Enabled { get; set; }
    public string SmtpHost { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string SenderName { get; set; } = "MotorMart";
    public string SenderEmail { get; set; } = "no-reply@motormart.local";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}
