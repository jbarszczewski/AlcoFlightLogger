using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AlcoFlightLogger.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public int PilotId { get; set; }
        public ICollection<FuelPoint> FuelPoints { get; set; }
    }    
}
