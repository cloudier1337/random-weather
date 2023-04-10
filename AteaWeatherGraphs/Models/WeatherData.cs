using System;

namespace AteaWeatherGraphs.Models
{
    public class WeatherData
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Temperature { get; set; }
        public decimal WindSpeed { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}