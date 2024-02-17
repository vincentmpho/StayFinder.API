using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayFinder.API.Models;
using StayFinder.API.Models.DTOs.Hotel;
using StayFinder.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRepository _hotelsRepository;

        public HotelsController(IMapper mapper, IHotelsRepository hotelsRepository)
        {
            _mapper = mapper;
            _hotelsRepository = hotelsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            var hotels = await _hotelsRepository.GetAllAsync();
            var records = _mapper.Map<List<HotelDto>>(hotels);
            return StatusCode(StatusCodes.Status200OK, records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var hotelDto = _mapper.Map<HotelDto>(hotel);
            return StatusCode(StatusCodes.Status200OK, hotelDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostHotel(HotelDto hotelDto)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _hotelsRepository.AddAsync(hotel);

            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            await _hotelsRepository.DeleteAsync(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}