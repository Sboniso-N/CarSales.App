using System.ComponentModel.DataAnnotations;
using CarSales.Web.Models.Enums;

namespace CarSales.Web.Models;

public class Purchase
{
    public int PurchaseId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public int CarId { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public decimal AmountPaid { get; set; }
    public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.Pending;

    [StringLength(200)]
    public string? SalesReference { get; set; }

    public ApplicationUser? User { get; set; }
    public Car? Car { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
