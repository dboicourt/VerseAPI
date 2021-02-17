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
    public class ShipsController : ControllerBase
    {
        private readonly VerseContext _context;

        public ShipsController(VerseContext context)
        {
            _context = context;
        }

        // GET: api/Ships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ship>>> GetShip()
        {
            return await _context.Ship.ToListAsync();
        }

        // GET: api/Ships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ship>> GetShip(long id)
        {
            var ship = await _context.Ship.FindAsync(id);

            if (ship == null)
            {
                return NotFound();
            }

            return ship;
        }

        // PUT: api/Ships/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShip(long id, Ship ship)
        {
            if (id != ship.Id)
            {
                return BadRequest();
            }

            _context.Entry(ship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipExists(id))
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

        // POST: api/Ships
        [HttpPost]
        public async Task<ActionResult<Ship>> PostShip(AddShipPayload payload)
        {
            //generate unique ID
            long newId = 0;

            while (newId == 0 || ShipExists(newId))
            {
                var rnd = new Random().Next();
                newId = Convert.ToInt64(rnd);
            }

            //convert payload into new ship with starting values
            var ship = new Ship
            {
                Id = newId,
                ShipName = payload.ShipName,
                PilotName = payload.PilotName,
                Credits = 5000,
                Class = "Courier LLV",
                Ore = 0,
                Water = 0,
                Fuel = 500,
                Components = 0,
                Capacity = 1000
            };

            _context.Ship.Add(ship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShip", new { id = ship.Id }, ship);
        }

        // DELETE: api/Ships/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ship>> DeleteShip(long id)
        {
            var ship = await _context.Ship.FindAsync(id);
            if (ship == null)
            {
                return NotFound();
            }

            _context.Ship.Remove(ship);
            await _context.SaveChangesAsync();

            return ship;
        }

        private bool ShipExists(long id)
        {
            return _context.Ship.Any(e => e.Id == id);
        }
    }
}
