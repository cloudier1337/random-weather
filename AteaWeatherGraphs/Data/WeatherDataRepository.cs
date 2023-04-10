using AteaWeatherGraphs.Data;
using AteaWeatherGraphs.Models;
using Microsoft.EntityFrameworkCore;

public class WeatherDataRepository : IWeatherDataRepository
{
    private readonly WeatherDbContext _dbContext;

    public WeatherDataRepository(WeatherDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WeatherData>> GetWeatherDataAsync()
    {
        return await _dbContext.WeatherData.ToListAsync();
    }

    public async Task<WeatherData> GetWeatherDataByIdAsync(int id)
    {
        return await _dbContext.WeatherData.FindAsync(id);
    }

    public async Task AddWeatherDataAsync(WeatherData weatherData)
    {
        await _dbContext.WeatherData.AddAsync(weatherData);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateWeatherDataAsync(WeatherData weatherData)
    {
        _dbContext.Entry(weatherData).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteWeatherDataAsync(int id)
    {
        var weatherData = await _dbContext.WeatherData.FindAsync(id);
        if (weatherData != null)
        {
            _dbContext.WeatherData.Remove(weatherData);
            await _dbContext.SaveChangesAsync();
        }
    }
}