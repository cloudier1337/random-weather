using Microsoft.AspNetCore.Mvc;

namespace AteaWeatherGraphs.Controllers;

public class WeatherController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}