using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Services;

public class ApplicationDbInitializer(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        foreach (var role in new[] { "Admin", "Customer", "SalesAgent" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminEmail = "admin@motormart.local";
        var admin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                PhoneNumber = "+27 11 555 0199",
                Address = "Johannesburg Auto Centre",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin@12345");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        const string customerEmail = "customer@motormart.local";
        var customer = await userManager.Users.FirstOrDefaultAsync(u => u.Email == customerEmail);
        if (customer is null)
        {
            customer = new ApplicationUser
            {
                UserName = customerEmail,
                Email = customerEmail,
                FullName = "Demo Customer",
                PhoneNumber = "+27 82 123 4567",
                Address = "Sandton, Johannesburg",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(customer, "Customer@12345");
            await userManager.AddToRoleAsync(customer, "Customer");
        }

        if (await context.Cars.AnyAsync())
        {
            return;
        }

        var cars = new List<Car>
        {
            new()
            {
                Brand = "BMW",
                Model = "320d M Sport",
                Year = 2022,
                Price = 689999,
                Mileage = 28000,
                FuelType = FuelType.Diesel,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.Sedan,
                Condition = CarCondition.Used,
                Color = "Alpine White",
                Description = "Executive sedan with digital cockpit, leather trim, parking assist, and a full service history.",
                Status = CarStatus.Available,
                ImageUrl = "https://images.unsplash.com/photo-1555215695-3004980ad54e?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=80,https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 142
            },
            new()
            {
                Brand = "Toyota",
                Model = "Corolla Cross XR",
                Year = 2024,
                Price = 459900,
                Mileage = 5400,
                FuelType = FuelType.Petrol,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.SUV,
                Condition = CarCondition.New,
                Color = "Silver Metallic",
                Description = "Family-ready crossover with advanced safety, touchscreen infotainment, and low running costs.",
                Status = CarStatus.Available,
                ImageUrl = "https://images.unsplash.com/photo-1542282088-fe8426682b8f?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1519641471654-76ce0107ad1b?auto=format&fit=crop&w=1200&q=80,https://images.unsplash.com/photo-1553440569-bcc63803a83d?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 118
            },
            new()
            {
                Brand = "Ford",
                Model = "Ranger Wildtrak",
                Year = 2023,
                Price = 799900,
                Mileage = 18000,
                FuelType = FuelType.Diesel,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.Bakkie,
                Condition = CarCondition.Used,
                Color = "Orange Copper",
                Description = "Adventure-focused double cab with 4x4 capability, tow pack, canopy-ready bed, and premium trim.",
                Status = CarStatus.Available,
                ImageUrl = "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1502877338535-766e1452684a?auto=format&fit=crop&w=1200&q=80,https://images.unsplash.com/photo-1502161254066-6c74afbf07aa?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 203
            },
            new()
            {
                Brand = "Mercedes-Benz",
                Model = "C200 Avantgarde",
                Year = 2021,
                Price = 729500,
                Mileage = 36000,
                FuelType = FuelType.Hybrid,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.Sedan,
                Condition = CarCondition.Used,
                Color = "Obsidian Black",
                Description = "Luxury sedan with ambient lighting, mild hybrid efficiency, panoramic sunroof, and full digital displays.",
                Status = CarStatus.Available,
                ImageUrl = "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1507136566006-cfc505b114fc?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 91
            },
            new()
            {
                Brand = "Volkswagen",
                Model = "Polo GTI",
                Year = 2020,
                Price = 399900,
                Mileage = 49000,
                FuelType = FuelType.Petrol,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.Hatchback,
                Condition = CarCondition.Used,
                Color = "Pure White",
                Description = "Hot hatch with sporty handling, DSG gearbox, digital cluster, and excellent urban practicality.",
                Status = CarStatus.Available,
                ImageUrl = "https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1552519507-da3b142c6e3d?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 167
            },
            new()
            {
                Brand = "Volvo",
                Model = "XC40 Recharge",
                Year = 2024,
                Price = 949900,
                Mileage = 2200,
                FuelType = FuelType.Electric,
                Transmission = TransmissionType.Automatic,
                BodyType = BodyType.SUV,
                Condition = CarCondition.New,
                Color = "Sage Green",
                Description = "Premium electric SUV with all-wheel drive, Google built-in, safety suite, and instant torque.",
                Status = CarStatus.Reserved,
                ImageUrl = "https://images.unsplash.com/photo-1619682817481-e994891cd1f5?auto=format&fit=crop&w=1200&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1550355291-bbee04a92027?auto=format&fit=crop&w=1200&q=80",
                ViewCount = 74
            }
        };

        await context.Cars.AddRangeAsync(cars);
        await context.Bookings.AddAsync(new Booking
        {
            Car = cars[0],
            UserId = customer.Id,
            PreferredDate = DateTime.Today.AddDays(3),
            Status = BookingStatus.Pending,
            Notes = "Weekend morning slot preferred."
        });

        var purchase = new Purchase
        {
            Car = cars[1],
            UserId = customer.Id,
            AmountPaid = 25000,
            PaymentStatus = PaymentStatus.Submitted,
            PurchaseStatus = PurchaseStatus.Pending,
            SalesReference = "PO-2026-001"
        };

        purchase.Payments.Add(new Payment
        {
            PaymentMethod = "EFT",
            ProofOfPayment = "REF-889001",
            Status = PaymentStatus.Submitted
        });

        await context.Purchases.AddAsync(purchase);
        await context.Favorites.AddAsync(new Favorite { Car = cars[2], UserId = customer.Id });
        await context.ContactMessages.AddAsync(new ContactMessage
        {
            Name = "Lerato Nkosi",
            Email = "lerato@example.com",
            Subject = "Trade-in valuation",
            Message = "I would like to know if you accept trade-ins for SUVs and what documents are required."
        });

        await context.SaveChangesAsync();
    }
}
