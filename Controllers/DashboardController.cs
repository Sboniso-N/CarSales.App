using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

[Authorize]
public class DashboardController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        if (User.IsInRole("Admin") || User.IsInRole("SalesAgent"))
        {
            var adminModel = new AdminDashboardViewModel
            {
                TotalCars = await context.Cars.CountAsync(),
                TotalUsers = await context.Users.CountAsync(),
                PendingBookings = await context.Bookings.CountAsync(b => (int)b.Status == 1),
                Revenue = await context.Purchases
                    .Where(p => (int)p.PurchaseStatus == 4 || (int)p.PaymentStatus == 3)
                    .SumAsync(p => (decimal?)p.AmountPaid) ?? 0,
                PendingPurchases = await context.Purchases.CountAsync(p => (int)p.PurchaseStatus == 1),
                LowStockCars = await context.Cars.OrderBy(c => c.Status).ThenBy(c => c.DateAdded).Take(5).ToListAsync(),
                RecentBookings = await context.Bookings.Include(b => b.Car).Include(b => b.User).OrderByDescending(b => b.BookingDate).Take(5).ToListAsync(),
                RecentPurchases = await context.Purchases.Include(p => p.Car).Include(p => p.User).OrderByDescending(p => p.PurchaseDate).Take(5).ToListAsync(),
                RecentMessages = await context.ContactMessages.OrderByDescending(m => m.DateSent).Take(5).ToListAsync()
            };

            return View("Admin", adminModel);
        }

        var model = new CustomerDashboardViewModel
        {
            User = user,
            Favorites = await context.Favorites.Include(f => f.Car).Where(f => f.UserId == user.Id).ToListAsync(),
            Bookings = await context.Bookings.Include(b => b.Car).Where(b => b.UserId == user.Id).OrderByDescending(b => b.BookingDate).ToListAsync(),
            Purchases = await context.Purchases.Include(p => p.Car).Include(p => p.Payments).Where(p => p.UserId == user.Id).OrderByDescending(p => p.PurchaseDate).ToListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(string fullName, string? phoneNumber, string? address)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        user.FullName = fullName;
        user.PhoneNumber = phoneNumber;
        user.Address = address;

        await userManager.UpdateAsync(user);
        TempData["Success"] = "Your profile has been updated.";
        return RedirectToAction(nameof(Index));
    }
}
