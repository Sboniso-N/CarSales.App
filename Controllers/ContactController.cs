using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Web.Controllers;

public class ContactController(ApplicationDbContext context) : Controller
{
    public IActionResult Index() => View(new ContactMessageViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        context.ContactMessages.Add(new ContactMessage
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message
        });

        await context.SaveChangesAsync();
        TempData["Success"] = "Thanks for contacting us. Our sales team will reach out shortly.";
        return RedirectToAction(nameof(Index));
    }
}
