using CarSales.Web.Data;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using CarSales.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

public class HomeController(ApplicationDbContext context, ICarRepository carRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            FeaturedCars = await carRepository.GetFeaturedCarsAsync(3),
            LatestCars = await carRepository.GetLatestCarsAsync(6),
            SpecialOffers = await carRepository.GetSpecialOffersAsync(3),
            Brands = await carRepository.GetBrandsAsync(),
            TotalCars = await context.Cars.CountAsync(),
            AvailableCars = await context.Cars.CountAsync(c => c.Status == CarStatus.Available),
            SoldCars = await context.Cars.CountAsync(c => c.Status == CarStatus.Sold)
        };

        return View(model);
    }

    public IActionResult Privacy() => View();
}
