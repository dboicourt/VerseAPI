using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerseAPI.Models;

namespace VerseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanetsController : ControllerBase
    {
        private readonly VerseContext _context;

        public PlanetsController(VerseContext context)
        {
            _context = context;
        }

        // GET: api/Planets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> GetPlanet()
        {
            return await _context.Planet.ToListAsync();
        }

        // GET: api/Planets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Planet>> GetPlanet(long id)
        {
            var planet = await _context.Planet.FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            return planet;
        }

        // PUT: api/Planets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanet(long id, Planet planet)
        {
            if (id != planet.Id)
            {
                return BadRequest();
            }

            _context.Entry(planet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanetExists(id))
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

        // POST: api/Planets
        [HttpPost]
        public async Task<ActionResult<Planet>> PostPlanet(AddPlanetPayload payload)
        {
            //generate unique ID
            long newId = 0;

            while (newId == 0 || PlanetExists(newId))
            {
                var rnd = new Random().Next();
                newId = Convert.ToInt64(rnd);
            }

            //convert payload into new planet. Set 'market' price based on provided resource quantities
            var planet = new Planet
            {
                Id = newId,
                Name = payload.Name,
                Ore = SetResourcePrice(payload.OreCount),
                Water = SetResourcePrice(payload.WaterCount),
                Fuel = SetResourcePrice(payload.FuelCount),
                Components = SetResourcePrice(payload.ComponentsCount)
            };

            _context.Planet.Add(planet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanet", new { id = planet.Id }, planet);
        }

        // DELETE: api/Planets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Planet>> DeletePlanet(long id)
        {
            var planet = await _context.Planet.FindAsync(id);
            if (planet == null)
            {
                return NotFound();
            }

            _context.Planet.Remove(planet);
            await _context.SaveChangesAsync();

            return planet;
        }

        private bool PlanetExists(long id)
        {
            return _context.Planet.Any(e => e.Id == id);
        }

        private PlanetResource SetResourcePrice(long resourceAmount)
        {
            //imitate simple market values here (higher supply = lower value)
            var price = 100 - (long)Math.Round((decimal)(resourceAmount / 100));
            return new PlanetResource { Amount = resourceAmount, Price = price };
        }
    }
}
