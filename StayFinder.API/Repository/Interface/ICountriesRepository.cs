using StayFinder.API.Models;

namespace StayFinder.API.Repository.Interface
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);
    }
}
