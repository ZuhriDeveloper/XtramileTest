using Application.Queries;
using Application.Repositories;
using MassTransit;
using Application.Models;
using Application.Dtos;
using Domain;

namespace Application.Consumers
{
    public class WeatherQueryConsumer : IConsumer<GetWeatherQuery>
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly ILocationRepository _locationRepository;
        public WeatherQueryConsumer(IWeatherRepository weatherRepository,ILocationRepository locationRepository)
        {
            _weatherRepository = weatherRepository;
            _locationRepository = locationRepository;
        }
        public async Task Consume(ConsumeContext<GetWeatherQuery> context)
        {
            try
            {
                var weather = await _weatherRepository.GetWeatherByCityId(context.Message.CityId);
                if (weather == null)
                    throw new Exception("Data not found");

                var result = new WeatherDto();
                var city = await _locationRepository.GetCityById(context.Message.CityId);
                var country = await _locationRepository.GetCountryById(city.CountryId);

                result.TemperatureCelcius = weather.Temperature;
                result.TemperatureFahrenheit = ConvertCelciusToFahrenheit(result.TemperatureCelcius);
                result.Location = country.CountryName + ", " + city.CityName;
                result.Time = weather.Time.ToString();
                result.Wind = weather.Wind;
                result.Visibility = weather.Visibility;
                result.SkyCondition = weather.SkyCondition;
                result.DewPoint= weather.DewPoint;
                result.RelativeHumidity= weather.RelativeHumidity;
                result.Pressure = weather.Pressure;
                await context.RespondAsync(QueryResult.Ok(result));
            }
            catch (Exception ex)
            {
                await context.RespondAsync(QueryResult.Error(ex.Message));
            }
        }

        private double ConvertCelciusToFahrenheit(double temperature)
        {
            return 32 + (int)(temperature / 0.5556);
        }
    }
}
