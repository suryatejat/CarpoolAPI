using System;

namespace Carpool.API.Models
{
    public class Ride
    {
        public string RideId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public decimal Price { get; set; }
        public int SeatsAvailable { get; set; }
    }
}