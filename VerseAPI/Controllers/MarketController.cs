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
    [Route("api/Market")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly VerseContext _context;
        public MarketController(VerseContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<IActionResult> PurchaseResources(PurchasePayload payload)
        {
            var planet = await _context.Planet.FindAsync(payload.PlanetID);

            if (planet == null)
            {
                return NotFound();
            }

            var ship = await _context.Ship.FindAsync(payload.ShipID);

            if (ship == null)
            {
                return NotFound();
            }

            //transaction
            var res = payload.Resource;
            //fix below
            var planetRes = planet.GetType().GetProperty(res).GetValue(planet, null);
            var cost = payload.Quantity;

            _context.Entry(planet).State = EntityState.Modified;
            _context.Entry(ship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            //placeholder
            return NoContent();
        }
    }
}
