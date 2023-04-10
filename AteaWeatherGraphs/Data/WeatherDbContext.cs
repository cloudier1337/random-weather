using AteaWeatherGraphs.Models;
using Microsoft.EntityFrameworkCore;

namespace AteaWeatherGraphs.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherData> WeatherData { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=weatherdata.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the WeatherData entity
            modelBuilder.Entity<WeatherData>()
                .HasKey(wd => wd.Id);
        }
    }
}