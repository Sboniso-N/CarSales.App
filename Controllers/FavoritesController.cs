using CarSales.Web.Data;
using CarSales.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

[Authorize]
public class FavoritesController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        var favorites = await context.Favorites
            .Include(f => f.Car)
            .Where(f => f.UserId == user!.Id)
            .OrderByDescending(f => f.FavoriteId)
            .ToListAsync();

        return View(favorites);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int carId)
    {
        var user = await userManager.GetUserAsync(User);
        var favorite = await context.Favorites
            .FirstOrDefaultAsync(f => f.CarId == carId && f.UserId == user!.Id);

        if (favorite is null)
        {
            context.Favorites.Add(new Favorite { CarId = carId, UserId = user!.Id });
        }
        else
        {
            context.Favorites.Remove(favorite);
        }

        await context.SaveChangesAsync();
        return RedirectToAction("Details", "Cars", new { id = carId });
    }
}
