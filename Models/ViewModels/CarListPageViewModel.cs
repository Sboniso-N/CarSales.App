namespace CarSales.Web.Models.ViewModels;

public class CarListPageViewModel
{
    public CarFilterViewModel Filters { get; set; } = new();
    public PagedResult<Car> Cars { get; set; } = new();
    public IReadOnlyList<string> Brands { get; set; } = Array.Empty<string>();
}
