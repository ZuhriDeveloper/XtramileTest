using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Infrastructure.Seed;
using SQLitePCL;
using Application.Repositories;
using Infrastructure.Repositories;
using Application.Consumers;
using System;
using System.Data.SQLite;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IDbConnection>(provider =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnectionString");
                return new SqliteConnection(connectionString);
            });

            var connection = services.BuildServiceProvider().GetService<IDbConnection>();

            //this is just to make sure the db is created
            connection.Open();
            connection.Close();

            var seedLocation = new SeedLocation(connection);
            seedLocation.Seed();

            var seedWeather = new SeedWeather(connection);
            seedWeather.Seed();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IWeatherRepository, WeatherRepository>();

            return services;
        }
    }
}
