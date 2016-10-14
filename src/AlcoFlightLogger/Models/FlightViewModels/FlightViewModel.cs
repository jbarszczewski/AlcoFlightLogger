using System;
using System.Collections.Generic;

namespace AlcoFlightLogger.Models.FlightViewModels
{
    public class FlightViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public ICollection<FuelPointViewModel> FuelPoints { get; set; }
    }
}
