using Microsoft.EntityFrameworkCore;
using StayFinder.API.Data;
using StayFinder.API.Models;
using StayFinder.API.Repository.Interface;

namespace StayFinder.API.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly StayFinderDbContext _context;

        public CountriesRepository(StayFinderDbContext Context) : base(Context)
        {
            _context = Context;
        }

        public async Task<Country> GetDetailsAsync(int id)
        {
            return await _context.countries.Include(x => x.Hotels)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
