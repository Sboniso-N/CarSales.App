using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models.ViewModels;

public class BookingRequestViewModel
{
    public int CarId { get; set; }

    [Required]
    [Display(Name = "Preferred test drive date")]
    public DateTime PreferredDate { get; set; } = DateTime.Today.AddDays(1);

    [StringLength(500)]
    public string? Notes { get; set; }
}
