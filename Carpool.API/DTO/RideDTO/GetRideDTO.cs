using System;

namespace Carpool.API.DTO.RideDTO
{
    public class GetRideDTO
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public decimal Price { get; set; }
        public int SeatsAvailable { get; set; }
    }
}