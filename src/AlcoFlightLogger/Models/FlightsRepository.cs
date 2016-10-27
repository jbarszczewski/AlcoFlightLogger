using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlcoFlightLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AlcoFlightLogger.Models
{
    public class FlightsRepository : IFlightsRepository
    {
        private readonly ApplicationDbContext context;

        public FlightsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return this.context.Flights;
        }

        public IEnumerable<Flight> GetUserAllFlights(int id)
        {
            return this.context.Flights.Include(f => f.FuelPoints).Where(x => x.PilotId.Equals(id));
        }

        public async Task<Flight> GetFlightById(int id)
        {
            return await this.context.Flights.Include(f => f.FuelPoints).SingleOrDefaultAsync(m => m.FlightId.Equals(id));
        }
        
        public void AddFlight(Flight flight)
        {
            this.context.Add(flight).State = EntityState.Added;
        }

        public async Task<Flight> ModifyFlight(Flight flight)
        {
            var dbFlight = await this.context.Flights.SingleOrDefaultAsync(f => f.FlightId.Equals(flight.FlightId));
            if (dbFlight != null)
            {
                dbFlight.Name = flight.Name;
                dbFlight.Description = flight.Description;
                dbFlight.Score = flight.Score;
            }

            return dbFlight;
        }

        public Flight DeleteFlight(Flight flight)
        {
            this.context.Remove(flight).State = EntityState.Deleted;
            return flight;
        }

        public async Task<bool> SaveChanges()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }

        public void AddFuelPoint(FuelPoint fuelPointMapped)
        {
            this.context.Add(fuelPointMapped).State = EntityState.Added;
        }
    }
}
