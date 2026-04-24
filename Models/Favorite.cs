namespace CarSales.Web.Models;

public class Favorite
{
    public int FavoriteId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CarId { get; set; }

    public ApplicationUser? User { get; set; }
    public Car? Car { get; set; }
}
