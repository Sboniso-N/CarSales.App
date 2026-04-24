using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

[Authorize]
public class BookingsController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingRequestViewModel model)
    {
        if (User.IsInRole("Admin") || User.IsInRole("SalesAgent"))
        {
            TempData["Error"] = "Admin and sales accounts cannot create customer test drive bookings.";
            return RedirectToAction("Cars", "Admin");
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please select a valid preferred date for your test drive.";
            return RedirectToAction("Details", "Cars", new { id = model.CarId });
        }

        var user = await userManager.GetUserAsync(User);
        var car = await context.Cars.FirstOrDefaultAsync(c => c.CarId == model.CarId);
        if (user is null || car is null)
        {
            return NotFound();
        }

        if (car.Status != CarStatus.Available)
        {
            TempData["Error"] = "Only available vehicles can be booked for a test drive.";
            return RedirectToAction("Details", "Cars", new { id = model.CarId });
        }

        context.Bookings.Add(new Booking
        {
            CarId = model.CarId,
            UserId = user.Id,
            PreferredDate = model.PreferredDate,
            Notes = model.Notes
        });

        await context.SaveChangesAsync();
        TempData["Success"] = "Your test drive request has been submitted.";
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> Mine()
    {
        var user = await userManager.GetUserAsync(User);
        var bookings = await context.Bookings
            .Include(b => b.Car)
            .Where(b => b.UserId == user!.Id)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return View(bookings);
    }
}
