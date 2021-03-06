using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlcoFlightLogger.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Flights")]
    public class FlightsController : Controller
    {
        private readonly IFlightsRepository repository;
        private readonly UserManager<Pilot> userManager;

        public FlightsController(UserManager<Pilot> userManager, IFlightsRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        // GET: api/Flights
        [HttpGet("")]
        public async Task<IActionResult> GetFlights()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var flights = repository.GetUserAllFlights(user.Id);
            var flightVMs = Mapper.Map<IEnumerable<FlightViewModel>>(flights);
            return Ok(flightVMs);
        }

        // GET: api/Flights/id/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight(int id)
        {
            var flight = await repository.GetFlightById(id);
            var flightVM = Mapper.Map<FlightViewModel>(flight);
            return Ok(flightVM);
        }

        [HttpPut("")]
        public async Task<IActionResult> PutFlight([FromBody] FlightViewModel flightViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var flight = Mapper.Map<Flight>(flightViewModel);

            if (!this.FlightExists(flight.FlightId))
            {
                if (flight.PilotId == 0)
                {
                    var user = await userManager.GetUserAsync(HttpContext.User);
                    flight.PilotId = user.Id;
                }

                repository.AddFlight(flight);
            }
            else
            {
                flight = await this.repository.ModifyFlight(flight);
            }

            if (await this.repository.SaveChanges())
                return Ok(Mapper.Map<FlightViewModel>(flight));
            else
                return BadRequest("Couldn't save flight entry.");
        }

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
                var user = await userManager.GetUserAsync(HttpContext.User);
                var flights = repository.GetUserAllFlights(user.Id).ToList();
                var lastFlight = flights.LastOrDefault();
                if (lastFlight == null ||
                    (lastFlight.FuelPoints.Any() && lastFlight.FuelPoints.Last().Date.AddHours(4) < DateTime.Now))
                {
                    lastFlight = new Flight { Name = $"Flight on {DateTime.Now.DayOfWeek}", FuelPoints = new List<FuelPoint>(), PilotId = user.Id };
                    flights.Add(lastFlight);
                    repository.AddFlight(lastFlight);
                }

                fuelPointMapped.FlightId = lastFlight.FlightId;
                //lastFlight.FuelPoints.Add(fuelPointMapped);
                repository.AddFuelPoint(fuelPointMapped);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            if (await repository.SaveChanges())
                return Created($"api/Flights/FuelPoint{fuelPointMapped.FuelPointId}", fuelPointMapped);
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

            repository.DeleteFlight(flightMapped);
            if (flight == null)
            {
                return NotFound();
            }
            if (await repository.SaveChanges())
                return Ok(flightMapped);
            return BadRequest("Couldn't save new flight entry.");
        }

        private bool FlightExists(int id)
        {
            return repository.GetFlightById(id).Result != null;
        }
    }
}