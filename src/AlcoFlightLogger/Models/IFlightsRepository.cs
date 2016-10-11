using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlcoFlightLogger.Models
{
    public interface IFlightsRepository
    {
        IEnumerable<Flight> GetAllFlights();
        IEnumerable<Flight> GetUserAllFlights(int id);
        Task<Flight> GetFlightById(int id);
        void AddFlight(Flight flight);
        Flight DeleteFlight(Flight flight);
        Task<bool> SaveChanges();
        void AddFuelPoint(FuelPoint fuelPointMapped);
    }
}
