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
        private readonly IFlightEntriesRepository repository;

        public FlightEntriesController(IFlightEntriesRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/FlightEntries
        [HttpGet]
        public IEnumerable<FlightEntry> GetFlightEntries()
        {
            return this.repository.GetAllFlightEntries();
        }

        // GET: api/FlightEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlightEntry flightEntry = await this.repository.GetFlightEntryById(id);

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

            var user = await this.repository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.FlightEntries);
        }

        // POST: api/FlightEntries
        [HttpPost]
        public async Task<IActionResult> PostFlightEntry([FromBody] FlightEntry flightEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await this.repository.AddFlightEntry(flightEntry);
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

        // DELETE: api/FlightEntries/
        [HttpDelete()]
        public async Task<IActionResult> DeleteFlightEntry([FromBody] FlightEntry flightEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await this.repository.DeleteFlightEntry(flightEntry);
            if (flightEntry == null)
            {
                return NotFound();
            }

            return Ok(flightEntry);
        }

        private bool FlightEntryExists(int id)
        {
            return this.repository.GetAllFlightEntries().Any(e => e.FlightEntryId == id);
        }
    }
}