using System;

namespace AlcoFlightLogger.Models
{
    public class FlightEntry
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public LocationInfo Location { get; set; }
    }
}
