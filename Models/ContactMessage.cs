using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models;

public class ContactMessage
{
    [Key]
    public int MessageId { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(160)]
    public string Subject { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public DateTime DateSent { get; set; } = DateTime.UtcNow;
}
