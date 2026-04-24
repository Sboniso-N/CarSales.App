using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using CarSales.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

[Authorize]
public class PurchasesController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    ILogger<PurchasesController> logger) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseRequestViewModel model)
    {
        if (User.IsInRole("Admin") || User.IsInRole("SalesAgent"))
        {
            TempData["Error"] = "Admin and sales accounts cannot purchase vehicles.";
            return RedirectToAction("Cars", "Admin");
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please complete the purchase form correctly.";
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
            TempData["Error"] = "This vehicle is no longer available for purchase.";
            return RedirectToAction("Details", "Cars", new { id = model.CarId });
        }

        var hasOpenPurchase = await context.Purchases.AnyAsync(p =>
            p.CarId == model.CarId &&
            p.PurchaseStatus != PurchaseStatus.Rejected);

        if (hasOpenPurchase)
        {
            car.Status = CarStatus.Reserved;
            await context.SaveChangesAsync();
            TempData["Error"] = "This vehicle already has an active purchase request.";
            return RedirectToAction("Details", "Cars", new { id = model.CarId });
        }

        var paymentStatus = string.Equals(model.PaymentMethod, "Stripe Test", StringComparison.OrdinalIgnoreCase)
            ? PaymentStatus.Verified
            : string.IsNullOrWhiteSpace(model.ProofOfPayment)
                ? PaymentStatus.Pending
                : PaymentStatus.Submitted;

        car.Status = CarStatus.Reserved;

        var purchase = new Purchase
        {
            CarId = model.CarId,
            UserId = user.Id,
            AmountPaid = model.AmountPaid,
            PaymentStatus = paymentStatus,
            PurchaseStatus = PurchaseStatus.Pending,
            SalesReference = $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}"
        };

        purchase.Payments.Add(new Payment
        {
            PaymentMethod = model.PaymentMethod,
            ProofOfPayment = string.Equals(model.PaymentMethod, "Stripe Test", StringComparison.OrdinalIgnoreCase)
                ? $"Stripe Test approved with card ending {GetLastFourDigits(model.CardNumber)}"
                : model.ProofOfPayment,
            Status = paymentStatus
        });

        context.Purchases.Add(purchase);
        await context.SaveChangesAsync();

        var confirmationEmail = string.IsNullOrWhiteSpace(model.ConfirmationEmail) ? user.Email : model.ConfirmationEmail;
        if (!string.IsNullOrWhiteSpace(confirmationEmail))
        {
            try
            {
                await emailService.SendAsync(
                    confirmationEmail,
                    $"MotorMart purchase request {purchase.SalesReference}",
                    $"""
                    <p>Hello {user.FullName},</p>
                    <p>Your purchase request for <strong>{car.Brand} {car.Model}</strong> has been received.</p>
                    <p>Reference: <strong>{purchase.SalesReference}</strong></p>
                    <p>Payment method: <strong>{model.PaymentMethod}</strong></p>
                    <p>Status: <strong>{purchase.PurchaseStatus}</strong></p>
                    <p>Our team will contact you shortly with the next steps.</p>
                    """);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to send purchase request email for purchase {SalesReference}", purchase.SalesReference);
            }
        }

        TempData["Success"] = "Your purchase request has been submitted for review.";
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> Mine()
    {
        var user = await userManager.GetUserAsync(User);
        var purchases = await context.Purchases
            .Include(p => p.Car)
            .Include(p => p.Payments)
            .Where(p => p.UserId == user!.Id)
            .OrderByDescending(p => p.PurchaseDate)
            .ToListAsync();

        return View(purchases);
    }

    private static string GetLastFourDigits(string? cardNumber)
    {
        var normalizedCard = (cardNumber ?? string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
        return normalizedCard.Length >= 4 ? normalizedCard[^4..] : "4242";
    }
}
