using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayFinder.API.Data;
using StayFinder.API.Models;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly StayFinderDbContext _context;

        public CountriesController(StayFinderDbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> Getcountries()
        {
          if (_context.countries == null)
          {
              return NotFound();
          }
            return await _context.countries.ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
          if (_context.countries == null)
          {
              return NotFound();
          }
            var country = await _context.countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countries/5
    
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
          if (_context.countries == null)
          {
              return Problem("Entity set 'StayFinderDbContext.countries'  is null.");
          }
            _context.countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.countries == null)
            {
                return NotFound();
            }
            var country = await _context.countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
