using AteaWeatherGraphs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AteaWeatherGraphs.Controllers;

public class WeatherController : Controller
{
    private readonly IConfiguration _config;
    private readonly IWeatherDataRepository _repository;
    private readonly Timer _timer;

    public WeatherController(IWeatherDataRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;

        // Create timer with 60-second interval
        _timer = new Timer(OnTimerElapsed, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetMinTemperatureData()
    {
        // Retrieve weather data from repository
        var weatherData = await _repository.GetWeatherDataAsync();

        // Extract min temperature data for each city
        var minTemperatureData = weatherData.GroupBy(x => new { x.Country, x.City })
            .Select(g => new {
                g.Key.Country,
                g.Key.City,
                Temperature = g.Min(x => x.Temperature)
            });

        // Return data as JSON
        return Json(minTemperatureData);
    }

    [HttpGet]
    public async Task<IActionResult> GetHighestWindSpeedData()
    {
        // Retrieve weather data from repository
        var weatherData = await _repository.GetWeatherDataAsync();

        // Extract highest wind speed data for each city
        var highestWindSpeedData = weatherData.GroupBy(x => new { x.Country, x.City })
            .Select(g => new {
                g.Key.Country,
                g.Key.City,
                WindSpeed = g.Max(x => x.WindSpeed)
            });

        // Return data as JSON
        return Json(highestWindSpeedData);
    }

    private async void OnTimerElapsed(object state)
    {
        await GetCurrentWeatherData();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
        }
        base.Dispose(disposing);
    }
    
    public async Task<IActionResult> Index()
    {
        var weatherData = await _repository.GetWeatherDataAsync();
        return View(weatherData);
    }


    public async Task GetCurrentWeatherData()
    {
        var httpClient = new HttpClient();

        Dictionary<string, string[]> cityData = new Dictionary<string, string[]>
        {
            {"New York", new [] {"40.7128", "-74.0060", "United States"}},
            {"London", new [] {"51.5074", "-0.1278", "United Kingdom"}},
            {"Paris", new [] {"48.8566", "2.3522", "France"}},
            {"Tokyo", new [] {"35.6895", "139.6917", "Japan"}},
            {"Beijing", new [] {"39.9042", "116.4074", "China"}},
            {"Sydney", new [] {"-33.8688", "151.2093", "Australia"}},
            {"Moscow", new [] {"55.7558", "37.6173", "Russia"}},
            {"Rio de Janeiro", new [] {"-22.9068", "-43.1729", "Brazil"}},
            {"Cape Town", new [] {"-33.9249", "18.4241", "South Africa"}},
            {"Dubai", new [] {"25.2048", "55.2708", "United Arab Emirates"}}
        };


            foreach (var city in cityData.Keys)
            {
                var arrayData = cityData[city];
                var lat = arrayData[0];
                var lon = arrayData[1];
                var country = arrayData[2];

                var apiKey = _config["OpenWeatherMapApiKey"];
                
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JObject.Parse(json);
                    var weather = new WeatherData
                    {
                        Country = country,
                        City = city,
                        Temperature = obj["main"]["temp"].Value<decimal>(),
                        Clouds = obj["clouds"]["all"].Value<int>(),
                        WindSpeed = obj["wind"]["speed"].Value<decimal>(),
                        LastUpdate = DateTimeOffset.FromUnixTimeSeconds(obj["dt"].Value<long>()).UtcDateTime
                    };
                    await _repository.AddWeatherDataAsync(weather);
                }
            }
    }
}
