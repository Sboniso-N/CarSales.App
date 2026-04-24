using CarSales.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.CarId })
            .IsUnique();

        builder.Entity<Car>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

        builder.Entity<Purchase>()
            .Property(p => p.AmountPaid)
            .HasPrecision(18, 2);

        builder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Favorite>()
            .HasOne(f => f.Car)
            .WithMany(c => c.Favorites)
            .HasForeignKey(f => f.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Booking>()
            .HasOne(b => b.Car)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Purchase>()
            .HasOne(p => p.User)
            .WithMany(u => u.Purchases)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Purchase>()
            .HasOne(p => p.Car)
            .WithMany(c => c.Purchases)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Payment>()
            .HasOne(p => p.Purchase)
            .WithMany(pu => pu.Payments)
            .HasForeignKey(p => p.PurchaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
