﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerseAPI.Models;

namespace VerseAPI.Controllers
{
    [Route("Verse/[controller]")]
    [ApiController]
    public class PlanetsController : ControllerBase
    {
        private readonly VerseContext _context;

        public PlanetsController(VerseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> GetPlanet()
        {
            return await _context.Planet.ToListAsync();
        }

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
                OreAmount = payload.OreCount,
                OrePrice = SetResourcePrice(payload.OreCount),
                WaterAmount = payload.WaterCount,
                WaterPrice = SetResourcePrice(payload.WaterCount),
                FuelAmount = payload.FuelCount,
                FuelPrice = SetResourcePrice(payload.FuelCount),
                ComponentsPrice = SetResourcePrice(payload.ComponentsCount)
            };

            _context.Planet.Add(planet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanet", new { id = planet.Id }, planet);
        }

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

        //takes value for resource amount and sets the appropriate cost
        private long SetResourcePrice(long resourceAmount)
        {
            //imitate simple market values here (higher supply = lower value)
            var price = 100 - (long)Math.Round((decimal)(resourceAmount / 100));
            return price;
        }
    }
}
