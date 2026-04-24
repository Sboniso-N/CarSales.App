using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.Models.Enums;
using CarSales.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Repositories;

public class CarRepository(ApplicationDbContext context) : ICarRepository
{
    public async Task<PagedResult<Car>> GetPagedCarsAsync(CarFilterViewModel filter)
    {
        var query = context.Cars.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(c =>
                (c.Brand + " " + c.Model).Contains(filter.SearchTerm) ||
                c.Description.Contains(filter.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            query = query.Where(c => c.Brand == filter.Brand);
        }

        if (filter.Year.HasValue)
        {
            query = query.Where(c => c.Year == filter.Year.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(c => c.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(c => c.Price <= filter.MaxPrice.Value);
        }

        if (filter.BodyType.HasValue)
        {
            query = query.Where(c => c.BodyType == filter.BodyType.Value);
        }

        if (filter.FuelType.HasValue)
        {
            query = query.Where(c => c.FuelType == filter.FuelType.Value);
        }

        if (filter.Transmission.HasValue)
        {
            query = query.Where(c => c.Transmission == filter.Transmission.Value);
        }

        if (filter.Condition.HasValue)
        {
            query = query.Where(c => c.Condition == filter.Condition.Value);
        }

        if (filter.AvailableOnly)
        {
            query = query.Where(c => c.Status == CarStatus.Available);
        }

        query = filter.SortOrder switch
        {
            "price_asc" => query.OrderBy(c => c.Price),
            "price_desc" => query.OrderByDescending(c => c.Price),
            "popular" => query.OrderByDescending(c => c.ViewCount),
            _ => query.OrderByDescending(c => c.DateAdded)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Car>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IReadOnlyList<Car>> GetFeaturedCarsAsync(int count) =>
        await context.Cars.AsNoTracking()
            .Where(c => c.Status == CarStatus.Available)
            .OrderByDescending(c => c.ViewCount)
            .Take(count)
            .ToListAsync();

    public async Task<IReadOnlyList<Car>> GetLatestCarsAsync(int count) =>
        await context.Cars.AsNoTracking()
            .Where(c => c.Status == CarStatus.Available)
            .OrderByDescending(c => c.DateAdded)
            .Take(count)
            .ToListAsync();

    public async Task<IReadOnlyList<Car>> GetSpecialOffersAsync(int count) =>
        await context.Cars.AsNoTracking()
            .Where(c => c.Status == CarStatus.Available)
            .OrderBy(c => c.Price)
            .Take(count)
            .ToListAsync();

    public async Task<IReadOnlyList<string>> GetBrandsAsync() =>
        await context.Cars.AsNoTracking()
            .Select(c => c.Brand)
            .Distinct()
            .OrderBy(b => b)
            .ToListAsync();

    public Task<Car?> GetByIdAsync(int id) =>
        context.Cars.FirstOrDefaultAsync(c => c.CarId == id);

    public async Task<IReadOnlyList<Car>> GetSimilarCarsAsync(Car car, int count) =>
        await context.Cars.AsNoTracking()
            .Where(c => c.CarId != car.CarId && c.Brand == car.Brand)
            .OrderByDescending(c => c.DateAdded)
            .Take(count)
            .ToListAsync();

    public async Task AddAsync(Car car)
    {
        context.Cars.Add(car);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Car car)
    {
        context.Cars.Remove(car);
        await context.SaveChangesAsync();
    }
}
