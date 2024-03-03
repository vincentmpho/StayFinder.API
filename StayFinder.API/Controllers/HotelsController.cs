using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StayFinder.API.Models;
using StayFinder.API.Models.DTOs.Hotel;
using StayFinder.API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRepository _hotelsRepository;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IMapper mapper, IHotelsRepository hotelsRepository, ILogger<HotelsController> logger)
        {
            _mapper = mapper;
            _hotelsRepository = hotelsRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _hotelsRepository.GetAllAsync();
                var records = _mapper.Map<List<HotelDto>>(hotels);
                return StatusCode(StatusCodes.Status200OK, records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve hotels");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve hotels");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _hotelsRepository.GetAsync(id);
                if (hotel == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var hotelDto = _mapper.Map<HotelDto>(hotel);
                return StatusCode(StatusCodes.Status200OK, hotelDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve hotel with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve hotel");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostHotel(HotelDto hotelDto)
        {
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _hotelsRepository.AddAsync(hotel);
                return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create hotel");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create hotel");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            try
            {
                if (id != hotelDto.Id)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid Record Id");
                }

                var existingHotel = await _hotelsRepository.GetAsync(id);
                if (existingHotel == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _mapper.Map(hotelDto, existingHotel);
                await _hotelsRepository.UpdateAsync(existingHotel);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update hotel with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update hotel");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var hotel = await _hotelsRepository.GetAsync(id);
                if (hotel == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                await _hotelsRepository.DeleteAsync(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete hotel with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete hotel");
            }
        }
    }
}
