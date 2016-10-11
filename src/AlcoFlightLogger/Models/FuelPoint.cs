using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoFlightLogger.Models
{
    public class FuelPoint
    {
        public int FuelPointId { get; set; }
        public DateTime Date { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int FlightId { get; set; }
    }
}
