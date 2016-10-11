using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace AlcoFlightLogger.Models
{
    // Add profile data for application users by adding properties to the Pilot class
    public class Pilot : IdentityUser<int>
    {
        public ICollection<Flight> Flights { get; set; }
    }
}