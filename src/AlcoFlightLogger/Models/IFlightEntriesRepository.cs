using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlcoFlightLogger.Models
{
    public interface IFlightEntriesRepository
    {
        IEnumerable<FlightEntry> GetAllFlightEntries();
        IEnumerable<FlightEntry> GetUserAllFlightEntries(string name);
        Task<FlightEntry> GetFlightEntryById(int id);
        void AddFlightEntry(FlightEntry flightEntry);
        Task<FlightEntry> DeleteFlightEntry(FlightEntry flightEntry);
        Task<bool> SaveChanges();
    }
}
