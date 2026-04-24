namespace CarSales.Web.Models.ViewModels;

public class CarDetailsViewModel
{
    public Car Car { get; set; } = default!;
    public IReadOnlyList<Car> SimilarCars { get; set; } = Array.Empty<Car>();
    public bool IsFavorite { get; set; }
    public bool CanCustomerInteract { get; set; }
    public bool IsAdminViewer { get; set; }
    public decimal EstimatedMonthlyPayment { get; set; }
    public IReadOnlyList<string> GalleryImages { get; set; } = Array.Empty<string>();
}
