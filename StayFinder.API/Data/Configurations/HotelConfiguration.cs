using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.API.Models;

namespace StayFinder.API.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
