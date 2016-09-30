using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightEntryViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AlcoFlightLogger.Controllers
{
    [Authorize]
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
        [HttpGet("")]
        public IActionResult GetFlightEntries()
        {
            return Ok(Mapper.Map<IEnumerable<FlightEntryViewModel>>(this.repository.GetUserAllFlightEntries(User.Identity.Name)));
        }
        
        // POST: api/FlightEntries
        [HttpPost("")]
        public async Task<IActionResult> PostFlightEntry([FromBody] FlightEntryViewModel flightEntry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var flightEntryMapped = Mapper.Map<FlightEntry>(flightEntry);

            try
            {
                this.repository.AddFlightEntry(flightEntryMapped);
            }
            catch (DbUpdateException)
            {
                if (FlightEntryExists(flightEntryMapped.FlightEntryId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            if (await this.repository.SaveChanges())
                return Created($"api/FlightEntries/{flightEntryMapped.FlightEntryId}", flightEntry);
            else
                return BadRequest("Couldn't save new flight entry.");
        }

        // DELETE: api/FlightEntries/
        [HttpDelete("")]
        public async Task<IActionResult> DeleteFlightEntry([FromBody] FlightEntryViewModel flightEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var flightEntryMapped = Mapper.Map<FlightEntry>(flightEntry);

            await this.repository.DeleteFlightEntry(flightEntryMapped);
            if (flightEntry == null)
            {
                return NotFound();
            }
            if (await this.repository.SaveChanges())
                return Ok(flightEntryMapped);
            else
                return BadRequest("Couldn't save new flight entry.");
        }

        private bool FlightEntryExists(int id)
        {
            return this.repository.GetFlightEntryById(id).Result != null;
        }
    }
}