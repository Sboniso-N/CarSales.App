namespace CarSales.Web.Models.ViewModels;

public class HomeViewModel
{
    public IReadOnlyList<Car> FeaturedCars { get; set; } = Array.Empty<Car>();
    public IReadOnlyList<Car> LatestCars { get; set; } = Array.Empty<Car>();
    public IReadOnlyList<Car> SpecialOffers { get; set; } = Array.Empty<Car>();
    public IReadOnlyList<string> Brands { get; set; } = Array.Empty<string>();
    public int TotalCars { get; set; }
    public int AvailableCars { get; set; }
    public int SoldCars { get; set; }
}
