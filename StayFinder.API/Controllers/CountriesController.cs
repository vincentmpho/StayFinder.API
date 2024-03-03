using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StayFinder.API.Models;
using StayFinder.API.Models.DTOs.Country;
using StayFinder.API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger)
        {
            _mapper = mapper;
            _countriesRepository = countriesRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _countriesRepository.GetAllAsync();
                var records = _mapper.Map<List<CountryDto>>(countries);
                return StatusCode(StatusCodes.Status200OK, records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve countries");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve countries");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _countriesRepository.GetDetailsAsync(id);
                if (country == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var countryDto = _mapper.Map<CountryDto>(country);
                return StatusCode(StatusCodes.Status200OK, countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve country with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve country");
            }
        }

        //POST : api/Countires
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCountry([FromBody] CreateCountryDto createCountryDto)
        {
            try
            {
                var country = _mapper.Map<Country>(createCountryDto);
                await _countriesRepository.AddAsync(country);
                return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create country");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create country");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, [FromBody] UpdateCountryDto updateCountryDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update country with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update country");
            }
        }

        //DELETE : api/Countries
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                var country = await _countriesRepository.GetAsync(id);
                if (country == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                await _countriesRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete country with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete country");
            }
        }
    }
}
