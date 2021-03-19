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
    [Route("Verse/Market")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly VerseContext _context;
        public MarketController(VerseContext context)
        {
            _context = context;
        }

        [HttpPut("Purchase")]
        public async Task<ActionResult<Ship>> PurchaseResources(PurchasePayload payload)
        {
            Planet planet = await _context.Planet.FindAsync(payload.PlanetID);

            if (planet == null)
            {
                return NotFound();
            }

            var ship = await _context.Ship.FindAsync(payload.ShipID);

            if (ship == null)
            {
                return NotFound();
            }

            //transaction checks

            //get planet resource info
            var planetResAmount = planet.GetType().GetProperty($"{payload.Resource}Amount").GetValue(planet, null);
            long resAmount = (long)planetResAmount;
            long resourceTotal = resAmount - payload.Quantity;

            //check available amount vs payload
            if (resourceTotal < 0)
            {
                return BadRequest($"{planet.Name} does not have sufficient {payload.Resource} for transaction");
            }

            //find out total cost and check against ship balance
            var planetResRate = planet.GetType().GetProperty($"{payload.Resource}Price").GetValue(planet, null);
            long resRate = (long)planetResRate;
            var totalCost = payload.Quantity * resRate;
            if (totalCost > ship.Credits)
            {
                return BadRequest($"{ship.PilotName} does not have sufficient credits for transaction");
            }

            //check available capacity of ship
            if (ship.Capacity < payload.Quantity)
            {
                return BadRequest($"{ship.ShipName} does not have enough remaining cargo capacity for transaction");
            }

            //enact transaction

            //remove resource from planet
            SetValue(planet, $"{payload.Resource}Amount", resourceTotal);

            //add resource to ship, lower remaining ship capacity, remove credits to pay for transaction
            long shipResourceTotal = SnagValue(ship, payload.Resource) + payload.Quantity;
            SetValue(ship, $"{payload.Resource}", shipResourceTotal);
            ship.Capacity = ship.Capacity - payload.Quantity;
            ship.Credits = ship.Credits - totalCost;


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

            return ship;
        }

        //helper functions to unpack values
        public static void SetValue(object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }

        long SnagValue(object obj, string propertyName)
        {
            var result = obj.GetType().GetProperty(propertyName).GetValue(obj);
            return (long)result;
        }
    }
}
