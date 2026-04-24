using System.ComponentModel.DataAnnotations;
using CarSales.Web.Models.Enums;

namespace CarSales.Web.Models;

public class Booking
{
    public int BookingId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public int CarId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public DateTime PreferredDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    [StringLength(500)]
    public string? Notes { get; set; }

    public ApplicationUser? User { get; set; }
    public Car? Car { get; set; }
}
