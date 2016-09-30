using System;

namespace AlcoFlightLogger.Models.FlightEntryViewModels
{
    public class FlightEntryViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}
