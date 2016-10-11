using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoFlightLogger.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AlcoFlightLogger.Models.FlightViewModels;
using System.Security.Claims;
using System.Linq;

namespace AlcoFlightLogger.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Flights")]
    public class FlightsController : Controller
    {
        private readonly UserManager<Pilot> userManager;
        private readonly IFlightsRepository repository;

        public FlightsController(UserManager<Pilot> userManager, IFlightsRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        // GET: api/Flights
        [HttpGet("")]
        public async Task<IActionResult> GetFlights()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var flights = this.repository.GetUserAllFlights(user.Id);
            var flightVMs = Mapper.Map<IEnumerable<FlightViewModel>>(flights);
            return Ok(flightVMs);
        }
        
        // POST: api/Flights
        //[HttpPost("")]
        //public async Task<IActionResult> PostFlight([FromBody] FlightViewModel flight)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
            
        //    var flightMapped = Mapper.Map<Flight>(flight);
        //   // flightMapped.UserName = User.Identity;

        //    try
        //    {
        //        this.repository.AddFlight(flightMapped);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (FlightExists(flightMapped.FlightId))
        //        {
        //            return new StatusCodeResult(StatusCodes.Status409Conflict);
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    if (await this.repository.SaveChanges())
        //        return Created($"api/Flights/{flightMapped.FlightId}", flight);
        //    else
        //        return BadRequest("Couldn't save new flight entry.");
        //}

        [HttpPost("FuelPoint")]
        public async Task<IActionResult> PostFuelPoint([FromBody] FuelPointViewModel fuelPoint)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fuelPointMapped = Mapper.Map<FuelPoint>(fuelPoint);
            if (fuelPointMapped.Date.Equals(DateTime.MinValue))
                fuelPointMapped.Date = DateTime.Now;

            try
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);
                var flights = this.repository.GetUserAllFlights(user.Id);
                var lastFlight = flights.Last();
                fuelPointMapped.FlightId = lastFlight.FlightId;
                lastFlight.FuelPoints.Add(fuelPointMapped);
                this.repository.AddFuelPoint(fuelPointMapped);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            if (await this.repository.SaveChanges())
                return Created($"api/Flights/FuelPoint{fuelPointMapped.FuelPointId}", fuelPointMapped);
            else
                return BadRequest("Couldn't save new flight entry.");
        }

        // DELETE: api/Flights/
        [HttpDelete("")]
        public async Task<IActionResult> DeleteFlight([FromBody] FlightViewModel flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var flightMapped = Mapper.Map<Flight>(flight);

            this.repository.DeleteFlight(flightMapped);
            if (flight == null)
            {
                return NotFound();
            }
            if (await this.repository.SaveChanges())
                return Ok(flightMapped);
            else
                return BadRequest("Couldn't save new flight entry.");
        }

        private bool FlightExists(int id)
        {
            return this.repository.GetFlightById(id).Result != null;
        }
    }
}