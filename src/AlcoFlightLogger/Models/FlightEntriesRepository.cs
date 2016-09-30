using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlcoFlightLogger.Data;
using Microsoft.EntityFrameworkCore;

namespace AlcoFlightLogger.Models
{
    public class FlightEntriesRepository : IFlightEntriesRepository
    {
        private readonly ApplicationDbContext context;

        public FlightEntriesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<FlightEntry> GetAllFlightEntries()
        {
            return this.context.FlightEntries;
        }

        public IEnumerable<FlightEntry> GetUserAllFlightEntries(string name)
        {
            return this.context.FlightEntries.Where(x => x.UserName.Equals(name));
        }

        public async Task<FlightEntry> GetFlightEntryById(int id)
        {
            return await this.context.FlightEntries.SingleOrDefaultAsync(m => m.FlightEntryId.Equals(id));
        }
        
        public void AddFlightEntry(FlightEntry flightEntry)
        {
            this.context.Add(flightEntry);
        }

        public FlightEntry DeleteFlightEntry(FlightEntry flightEntry)
        {
            this.context.Remove(flightEntry).State = EntityState.Deleted;
            return flightEntry;
        }

        public async Task<bool> SaveChanges()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }
    }
}
