using AteaWeather.Services.Services;
using AteaWeather.Services.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace AteaWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped<ITableService, TableService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AteaWeather API", Version = "v1" });
            });

            var app = builder.Build();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AteaWeather API v1");
            });

            app.Run();
        }
    }
}