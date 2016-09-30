using System;

namespace AlcoFlightLogger.Models.FlightEntryViewModels
{
    public class FlightEntryViewModel
    {
        public DateTime Date { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}
