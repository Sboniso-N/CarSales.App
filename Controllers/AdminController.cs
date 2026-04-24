using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using CarSales.Web.Repositories;
using CarSales.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

[Authorize(Roles = "Admin,SalesAgent")]
public class AdminController(
    ApplicationDbContext context,
    ICarRepository carRepository,
    IEmailService emailService,
    ILogger<AdminController> logger) : Controller
{
    public async Task<IActionResult> Cars()
    {
        var cars = await context.Cars.OrderByDescending(c => c.DateAdded).ToListAsync();
        return View(cars);
    }

    public IActionResult CreateCar() => View("CarForm", new CarFormViewModel { Year = DateTime.Today.Year });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCar(CarFormViewModel form)
    {
        if (!ModelState.IsValid)
        {
            return View("CarForm", form);
        }

        var car = Map(form, new Car());
        await carRepository.AddAsync(car);
        TempData["Success"] = "Car listing created successfully.";
        return RedirectToAction(nameof(Cars));
    }

    public async Task<IActionResult> EditCar(int id)
    {
        var car = await carRepository.GetByIdAsync(id);
        if (car is null)
        {
            return NotFound();
        }

        return View("CarForm", new CarFormViewModel
        {
            CarId = car.CarId,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            Mileage = car.Mileage,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            BodyType = car.BodyType,
            Condition = car.Condition,
            Status = car.Status,
            Color = car.Color,
            Description = car.Description,
            ImageUrl = car.ImageUrl,
            GalleryImages = car.GalleryImages
        });
    }

    public async Task<IActionResult> CarDetails(int id)
    {
        var car = await context.Cars
            .Include(c => c.Bookings)
            .Include(c => c.Purchases)
            .FirstOrDefaultAsync(c => c.CarId == id);

        if (car is null)
        {
            return NotFound();
        }

        var model = new CarDetailsViewModel
        {
            Car = car,
            SimilarCars = await carRepository.GetSimilarCarsAsync(car, 3),
            IsFavorite = false,
            CanCustomerInteract = false,
            IsAdminViewer = true,
            EstimatedMonthlyPayment = Math.Round((car.Price * 1.12m) / 60m, 2),
            GalleryImages = string.IsNullOrWhiteSpace(car.GalleryImages)
                ? new[] { car.ImageUrl }
                : car.GalleryImages.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCar(CarFormViewModel form)
    {
        if (!ModelState.IsValid || !form.CarId.HasValue)
        {
            return View("CarForm", form);
        }

        var car = await carRepository.GetByIdAsync(form.CarId.Value);
        if (car is null)
        {
            return NotFound();
        }

        Map(form, car);
        await carRepository.UpdateAsync(car);
        TempData["Success"] = "Car listing updated successfully.";
        return RedirectToAction(nameof(Cars));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await carRepository.GetByIdAsync(id);
        if (car is not null)
        {
            await carRepository.DeleteAsync(car);
        }

        TempData["Success"] = "Car listing deleted.";
        return RedirectToAction(nameof(Cars));
    }

    public async Task<IActionResult> Bookings()
    {
        var bookings = await context.Bookings.Include(b => b.Car).Include(b => b.User).OrderByDescending(b => b.BookingDate).ToListAsync();
        return View(bookings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateBookingStatus(int id, BookingStatus status)
    {
        var booking = await context.Bookings.FindAsync(id);
        if (booking is not null)
        {
            booking.Status = status;
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Bookings));
    }

    public async Task<IActionResult> Purchases()
    {
        var purchases = await context.Purchases.Include(p => p.Car).Include(p => p.User).Include(p => p.Payments).OrderByDescending(p => p.PurchaseDate).ToListAsync();
        return View(purchases);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePurchaseStatus(int id, PurchaseStatus status, PaymentStatus paymentStatus)
    {
        var purchase = await context.Purchases
            .Include(p => p.Car)
            .Include(p => p.User)
            .Include(p => p.Payments)
            .FirstOrDefaultAsync(p => p.PurchaseId == id);

        if (purchase is not null)
        {
            purchase.PurchaseStatus = status;
            purchase.PaymentStatus = paymentStatus;

            if (status == PurchaseStatus.Completed && purchase.Car is not null)
            {
                purchase.Car.Status = CarStatus.Sold;
            }
            else if (status == PurchaseStatus.Rejected && purchase.Car is not null)
            {
                purchase.Car.Status = CarStatus.Available;
            }
            else if (purchase.Car is not null && status is PurchaseStatus.Pending or PurchaseStatus.Approved)
            {
                purchase.Car.Status = CarStatus.Reserved;
            }

            foreach (var payment in purchase.Payments)
            {
                payment.Status = paymentStatus;
            }

            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(purchase.User?.Email))
            {
                try
                {
                    var carName = purchase.Car is null ? "your selected vehicle" : $"{purchase.Car.Brand} {purchase.Car.Model}";
                    await emailService.SendAsync(
                        purchase.User.Email!,
                        $"MotorMart purchase update {purchase.SalesReference}",
                        $"""
                        <p>Hello {purchase.User.FullName},</p>
                        <p>Your purchase request for <strong>{carName}</strong> has been updated.</p>
                        <p>Reference: <strong>{purchase.SalesReference}</strong></p>
                        <p>Purchase status: <strong>{purchase.PurchaseStatus}</strong></p>
                        <p>Payment status: <strong>{purchase.PaymentStatus}</strong></p>
                        <p>Thank you for choosing MotorMart.</p>
                        """);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to send purchase status email for purchase {PurchaseId}", purchase.PurchaseId);
                }
            }
        }

        return RedirectToAction(nameof(Purchases));
    }

    public async Task<IActionResult> Users()
    {
        var users = await context.Users.OrderByDescending(u => u.DateCreated).ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> Messages()
    {
        var messages = await context.ContactMessages.OrderByDescending(m => m.DateSent).ToListAsync();
        return View(messages);
    }

    private static Car Map(CarFormViewModel model, Car car)
    {
        car.Brand = model.Brand;
        car.Model = model.Model;
        car.Year = model.Year;
        car.Price = model.Price;
        car.Mileage = model.Mileage;
        car.FuelType = model.FuelType;
        car.Transmission = model.Transmission;
        car.BodyType = model.BodyType;
        car.Condition = model.Condition;
        car.Status = model.Status;
        car.Color = model.Color;
        car.Description = model.Description;
        car.ImageUrl = model.ImageUrl;
        car.GalleryImages = model.GalleryImages;
        return car;
    }
}
