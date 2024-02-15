using AutoMapper;
using StayFinder.API.Models;
using StayFinder.API.Models.DTOs.Country;
using StayFinder.API.Models.DTOs.Hotel;

namespace StayFinder.API.Configaration
{
    public class MapperConfig :Profile
    {
        public MapperConfig()
        {
                CreateMap<Country, CreateCountryDto>().ReverseMap();
                CreateMap<Country, GetCountryDto>().ReverseMap();
                CreateMap<Country, CountryDto>().ReverseMap();
                CreateMap<Country, UpdateCountryDto>().ReverseMap();


                CreateMap<Hotel, HotelDto>().ReverseMap();
                CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        }
    }
}
