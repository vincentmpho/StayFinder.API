using Microsoft.EntityFrameworkCore;
using StayFinder.API.Models;

namespace StayFinder.API.Data
{
    public class StayFinderDbContext : DbContext
    {
        public StayFinderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> countries { get; set; }
    }
}
