using System.ComponentModel.DataAnnotations;
using CarSales.Web.Models.Enums;

namespace CarSales.Web.Models.ViewModels;

public class CarFormViewModel
{
    public int? CarId { get; set; }

    [Required, StringLength(80)]
    public string Brand { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Model { get; set; } = string.Empty;

    [Range(1990, 2100)]
    public int Year { get; set; }

    [Range(0, 100000000)]
    public decimal Price { get; set; }

    [Range(0, 2000000)]
    public int Mileage { get; set; }

    public FuelType FuelType { get; set; }
    public TransmissionType Transmission { get; set; }
    public BodyType BodyType { get; set; }
    public CarCondition Condition { get; set; }
    public CarStatus Status { get; set; } = CarStatus.Available;

    [StringLength(40)]
    public string Color { get; set; } = string.Empty;

    [Required, StringLength(2500)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(500)]
    [Display(Name = "Primary image URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "Gallery image URLs (comma separated)")]
    public string? GalleryImages { get; set; }
}
