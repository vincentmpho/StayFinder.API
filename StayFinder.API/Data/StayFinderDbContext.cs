using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayFinder.API.Models;

namespace StayFinder.API.Data
{
    public class StayFinderDbContext : IdentityDbContext<ApiUser>
    {
        public StayFinderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamica",
                    ShortName = "JM"
                },
                  new Country
                  {
                      Id = 2,
                      Name = "South Africa",
                      ShortName = "SA"
                  },
                    new Country
                    {
                        Id = 3,
                        Name = "South Africa",
                        ShortName = "SA"
                    }
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.2
                },
                  new Hotel
                  {
                      Id = 2,
                      Name = "Thaba Pitsi Nature Reserve and Spa",
                      Address = "Limpopo",
                      CountryId = 3,
                      Rating = 5
                  },
                    new Hotel
                    {
                        Id = 3,
                        Name = "Table Bay Hotel",
                        Address = "Cape Town",
                        CountryId = 2,
                        Rating = 4.2
                    }
                );
        }
    }

   
}
