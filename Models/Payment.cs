using System.ComponentModel.DataAnnotations;
using CarSales.Web.Models.Enums;

namespace CarSales.Web.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public int PurchaseId { get; set; }

    [Required, StringLength(60)]
    public string PaymentMethod { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ProofOfPayment { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public Purchase? Purchase { get; set; }
}
