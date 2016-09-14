using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AlcoFlightLogger.Models
{
    public class FlightEntry
    {
        public int FlightEntryId { get; set; }
        
        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
        
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
