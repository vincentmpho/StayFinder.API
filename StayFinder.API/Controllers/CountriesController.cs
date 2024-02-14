using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayFinder.API.Models;
using StayFinder.API.Models.DTOs.Country;
using StayFinder.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {
            _mapper = mapper;
            _countriesRepository = countriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<CountryDto>>(countries);
            return StatusCode(StatusCodes.Status200OK, records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetailsAsync(id);
            if (country == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var countryDto = _mapper.Map<CountryDto>(country);
            return StatusCode(StatusCodes.Status200OK, countryDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostCountry([FromBody] CreateCountryDto createCountryDto)
        {
            var country = _mapper.Map<Country>(createCountryDto);
            await _countriesRepository.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, [FromBody] UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Record Id");
            }

            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);
            await _countriesRepository.UpdateAsync(country);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}