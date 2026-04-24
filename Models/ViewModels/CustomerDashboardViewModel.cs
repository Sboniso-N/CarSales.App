namespace CarSales.Web.Models.ViewModels;

public class CustomerDashboardViewModel
{
    public ApplicationUser User { get; set; } = default!;
    public IReadOnlyList<Favorite> Favorites { get; set; } = Array.Empty<Favorite>();
    public IReadOnlyList<Booking> Bookings { get; set; } = Array.Empty<Booking>();
    public IReadOnlyList<Purchase> Purchases { get; set; } = Array.Empty<Purchase>();
}
