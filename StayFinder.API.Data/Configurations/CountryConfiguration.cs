using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.API.Models;

namespace StayFinder.API.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
