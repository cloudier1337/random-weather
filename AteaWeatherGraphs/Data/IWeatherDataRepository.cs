using AteaWeatherGraphs.Models;

public interface IWeatherDataRepository
{
    Task<IEnumerable<WeatherData>> GetWeatherDataAsync();
    Task<WeatherData> GetWeatherDataByIdAsync(int id);
    Task AddWeatherDataAsync(WeatherData weatherData);
}