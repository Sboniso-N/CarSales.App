using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CarSales.Web.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
