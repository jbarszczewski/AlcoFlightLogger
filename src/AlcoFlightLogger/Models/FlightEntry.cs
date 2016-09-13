using System;
using System.ComponentModel.DataAnnotations;

namespace AlcoFlightLogger.Models
{
    public class FlightEntry
    {
        [Key]
        public int FlightEntryId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}
