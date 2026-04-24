using CarSales.Web.Models;
using CarSales.Web.Models.ViewModels;

namespace CarSales.Web.Repositories;

public interface ICarRepository
{
    Task<PagedResult<Car>> GetPagedCarsAsync(CarFilterViewModel filter);
    Task<IReadOnlyList<Car>> GetFeaturedCarsAsync(int count);
    Task<IReadOnlyList<Car>> GetLatestCarsAsync(int count);
    Task<IReadOnlyList<Car>> GetSpecialOffersAsync(int count);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<Car?> GetByIdAsync(int id);
    Task<IReadOnlyList<Car>> GetSimilarCarsAsync(Car car, int count);
    Task AddAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(Car car);
}
