using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayFinder.API.Data;
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

        // GET: api/Countries
        [HttpGet]
        public async Task<IActionResult> Getcountries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return StatusCode(StatusCodes.Status200OK, records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetailsAsync(id);

            //check
            if (country == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var countryDto = _mapper.Map<CountryDto>(country);
            return StatusCode(StatusCodes.Status200OK, countryDto);
        }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Record Id");
            }

            //_context.Entry(updateCountryDto).State = EntityState.Modified;

            var country = await _countriesRepository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);

            return NoContent();
        }

        // POST: api/Countries
        [HttpPost]
        public async Task<IActionResult> PostCountry(CreateCountryDto createCountryDto)
        {
            var country = _mapper.Map<Country>(createCountryDto);

            _countriesRepository.AddAsync(country);


            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
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

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}