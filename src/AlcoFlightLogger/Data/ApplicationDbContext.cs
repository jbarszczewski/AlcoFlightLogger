using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlcoFlightLogger.Models;

namespace AlcoFlightLogger.Data
{
    public class ApplicationDbContext : IdentityDbContext<Pilot, IdentityRole<int>, int>
    {
        public DbSet<Flight> Flights { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
