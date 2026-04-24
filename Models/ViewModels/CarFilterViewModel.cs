using CarSales.Web.Models.Enums;

namespace CarSales.Web.Models.ViewModels;

public class CarFilterViewModel
{
    public string? SearchTerm { get; set; }
    public string? Brand { get; set; }
    public int? Year { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public BodyType? BodyType { get; set; }
    public FuelType? FuelType { get; set; }
    public TransmissionType? Transmission { get; set; }
    public CarCondition? Condition { get; set; }
    public bool AvailableOnly { get; set; } = true;
    public string SortOrder { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;
}
