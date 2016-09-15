using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlcoFlightLogger.Models
{
    public interface IFlightEntriesRepository
    {
        IEnumerable<FlightEntry> GetAllFlightEntries();
        Task<FlightEntry> GetFlightEntryById(int id);
        Task<ApplicationUser> GetUserById(string id);
        Task AddFlightEntry(FlightEntry flightEntry);
        Task<FlightEntry> DeleteFlightEntry(FlightEntry flightEntry);
    }
}
