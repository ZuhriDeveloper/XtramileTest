using Application.Consumers;
using Application.Dtos;
using Application.Models;
using Application.Queries;
using Application.Repositories;
using Domain;
using MassTransit;
using Moq;
using System.Data;

[TestFixture]
public class WeatherQueryConsumerTests
{
    private WeatherQueryConsumer _weatherQueryConsumer;
    private Mock<IWeatherRepository> _weatherRepositoryMock;
    private Mock<ILocationRepository> _locationRepositoryMock;
    private Mock<ConsumeContext<GetWeatherQuery>> _consumeContextMock;
    private IDbConnection _connection;

    [SetUp]
    public void Setup()
    {
        ////var connectionFactory = new SqliteConnectionFactory(":memory:");
        //_connection = connectionFactory.CreateConnection();
        //_connection.Open();

        _weatherRepositoryMock = new Mock<IWeatherRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _consumeContextMock = new Mock<ConsumeContext<GetWeatherQuery>>();
        _weatherQueryConsumer = new WeatherQueryConsumer(_weatherRepositoryMock.Object, _locationRepositoryMock.Object);
    }

    [Test]
    public async Task Consume_WeatherFound_ReturnsOkQueryResult()
    {
        // Arrange
        var weather = new Weather
        {
            Temperature = 25,
            Time = DateTime.UtcNow,
            Wind = "S",
            Visibility = "10",
            SkyCondition = "Sunny",
            DewPoint = "10",
            RelativeHumidity = "20",
            Pressure = "200"
        };
        var city = new City
        {
            CityId = 1,
            CityName = "Jakarta",
            CountryId = 1
        };
        var country = new Country
        {
            CountryName = "Indonesia"
        };
        var message = new GetWeatherQuery
        {
            CityId = 1
        };
        var weatherRepositoryMock = new Mock<IWeatherRepository>();
        weatherRepositoryMock.Setup(x => x.GetWeatherByCityId(city.CityId)).ReturnsAsync(weather);

        var locationRepositoryMock = new Mock<ILocationRepository>();
        locationRepositoryMock.Setup(x => x.GetCityById(city.CityId)).ReturnsAsync(city);
        locationRepositoryMock.Setup(x => x.GetCountryById(city.CountryId)).ReturnsAsync(country);

        var contextMock = new Mock<ConsumeContext<GetWeatherQuery>>();
        var query = new GetWeatherQuery { CityId = city.CityId };
        contextMock.Setup(x => x.Message).Returns(query);

        var consumer = new WeatherQueryConsumer(weatherRepositoryMock.Object, locationRepositoryMock.Object);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        weatherRepositoryMock.Verify(x => x.GetWeatherByCityId(city.CityId), Times.Once);

        locationRepositoryMock.Verify(x => x.GetCityById(city.CityId), Times.Once);
        locationRepositoryMock.Verify(x => x.GetCountryById(city.CountryId), Times.Once);

        var expectedWeather = new WeatherDto
        {
            TemperatureCelcius = weather.Temperature,
            TemperatureFahrenheit = 68, // Expected Fahrenheit value for 20 Celsius
            Location = "UK, London",
            Time = weather.Time.ToString(),
            Wind = weather.Wind,
            Visibility = weather.Visibility,
            SkyCondition = weather.SkyCondition,
            DewPoint = weather.DewPoint,
            RelativeHumidity = weather.RelativeHumidity,
            Pressure = weather.Pressure
        };
        var expectedResponse = QueryResult.Ok(expectedWeather);
        contextMock.Verify(x => x.RespondAsync(expectedResponse), Times.Once);
    }

    [Test]
    public async Task Consume_WeatherNotFound_ReturnsErrorQueryResult()
    {
        // Arrange
        var message = new GetWeatherQuery
        {
            CityId = 1
        };
        _weatherRepositoryMock.Setup(x => x.GetWeatherByCityId(message.CityId)).ReturnsAsync((Weather)null);
        _consumeContextMock.Setup(x => x.Message).Returns(message);
        var expectedErrorMessage = "Data not found";
        var expectedErrorResult = QueryResult.Error(expectedErrorMessage);

        // Act
        await _weatherQueryConsumer.Consume(_consumeContextMock.Object);

        // Assert
        _consumeContextMock.Verify(x => x.RespondAsync(expectedErrorResult), Times.Once);
    }
}
