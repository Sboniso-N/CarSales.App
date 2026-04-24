namespace CarSales.Web.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalCars { get; set; }
    public int TotalUsers { get; set; }
    public int PendingBookings { get; set; }
    public decimal Revenue { get; set; }
    public int PendingPurchases { get; set; }
    public IReadOnlyList<Car> LowStockCars { get; set; } = Array.Empty<Car>();
    public IReadOnlyList<Booking> RecentBookings { get; set; } = Array.Empty<Booking>();
    public IReadOnlyList<Purchase> RecentPurchases { get; set; } = Array.Empty<Purchase>();
    public IReadOnlyList<ContactMessage> RecentMessages { get; set; } = Array.Empty<ContactMessage>();
}
