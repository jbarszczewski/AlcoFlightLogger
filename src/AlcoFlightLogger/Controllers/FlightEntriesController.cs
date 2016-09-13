using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoFlightLogger.Data;
using AlcoFlightLogger.Models;

namespace AlcoFlightLogger.Controllers
{
    [Produces("application/json")]
    [Route("api/FlightEntries")]
    public class FlightEntriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public FlightEntriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/FlightEntries
        [HttpGet]
        public IEnumerable<FlightEntry> GetFlightEntries()
        {
            return context.FlightEntries;
        }

        // GET: api/FlightEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlightEntry flightEntry = await context.FlightEntries.SingleOrDefaultAsync(m => m.FlightEntryId == id);

            if (flightEntry == null)
            {
                return NotFound();
            }

            return Ok(flightEntry);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserFlightEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flightEntry = await context.Users.SingleOrDefaultAsync(m => m.Id == id);

            if (flightEntry == null)
            {
                return NotFound();
            }

            return Ok(flightEntry.FlightEntries);
        }

        // PUT: api/FlightEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlightEntry([FromRoute] int id, [FromBody] FlightEntry flightEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flightEntry.FlightEntryId)
            {
                return BadRequest();
            }

            context.Entry(flightEntry).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightEntryExists(id))
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

        // POST: api/FlightEntries
        [HttpPost]
        public async Task<IActionResult> PostFlightEntry([FromBody] FlightEntry flightEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.FlightEntries.Add(flightEntry);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FlightEntryExists(flightEntry.FlightEntryId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFlightEntry", new { id = flightEntry.FlightEntryId }, flightEntry);
        }

        // DELETE: api/FlightEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlightEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlightEntry flightEntry = await context.FlightEntries.SingleOrDefaultAsync(m => m.FlightEntryId == id);
            if (flightEntry == null)
            {
                return NotFound();
            }

            context.FlightEntries.Remove(flightEntry);
            await context.SaveChangesAsync();

            return Ok(flightEntry);
        }

        private bool FlightEntryExists(int id)
        {
            return context.FlightEntries.Any(e => e.FlightEntryId == id);
        }
    }
}