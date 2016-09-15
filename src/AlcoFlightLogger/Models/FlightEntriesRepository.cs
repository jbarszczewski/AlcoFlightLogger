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

        public async Task<FlightEntry> GetFlightEntryById(int id)
        {
            return await context.FlightEntries.SingleOrDefaultAsync(m => m.FlightEntryId.Equals(id));
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await this.context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task AddFlightEntry(FlightEntry flightEntry)
        {
            this.context.Entry(flightEntry).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<FlightEntry> DeleteFlightEntry(FlightEntry flightEntry)
        {
            this.context.Remove(flightEntry).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return flightEntry;
        }
    }
}
