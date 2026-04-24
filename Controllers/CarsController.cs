using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using CarSales.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

public class CarsController(
    ICarRepository carRepository,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index([FromQuery] CarFilterViewModel filters)
    {
        if (User.IsInRole("Admin") || User.IsInRole("SalesAgent"))
        {
            return RedirectToAction("Cars", "Admin");
        }

        var model = new CarListPageViewModel
        {
            Filters = filters,
            Cars = await carRepository.GetPagedCarsAsync(filters),
            Brands = await carRepository.GetBrandsAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (User.IsInRole("Admin") || User.IsInRole("SalesAgent"))
        {
            return RedirectToAction("CarDetails", "Admin", new { id });
        }

        var car = await carRepository.GetByIdAsync(id);
        if (car is null)
        {
            return NotFound();
        }

        car.ViewCount++;
        await context.SaveChangesAsync();

        var user = await userManager.GetUserAsync(User);
        var isFavorite = user is not null &&
                         await context.Favorites.AnyAsync(f => f.CarId == id && f.UserId == user.Id);

        var model = new CarDetailsViewModel
        {
            Car = car,
            SimilarCars = await carRepository.GetSimilarCarsAsync(car, 3),
            IsFavorite = isFavorite,
            CanCustomerInteract = car.Status == CarStatus.Available,
            IsAdminViewer = false,
            EstimatedMonthlyPayment = Math.Round((car.Price * 1.12m) / 60m, 2),
            GalleryImages = string.IsNullOrWhiteSpace(car.GalleryImages)
                ? new[] { car.ImageUrl }
                : car.GalleryImages.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        };

        return View(model);
    }
}
